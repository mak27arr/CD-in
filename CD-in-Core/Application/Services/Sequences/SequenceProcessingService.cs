using CD_in_Core.Application.Services.Interfaces.Sequences;
using CD_in_Core.Application.Settings;
using CD_in_Core.Domain.Models.Sequences;
using CD_in_Core.Domain.Select;
using CD_in_Core.Infrastructure;

namespace CD_in_Core.Application.Services.Sequences
{
    internal class SequenceProcessingService : ISequenceProcessingService
    {
        private readonly ILargeNumberExtractionService _largeNumberExtractionService;
        private readonly IBeneficialReplacementService _beneficialReplacementService;
        private readonly ISubSequenceExtractorService _sequenceExtractorService;

        public SequenceProcessingService(
            ILargeNumberExtractionService largeNumberExtractionService,
            IBeneficialReplacementService beneficialReplacementService,
            ISubSequenceExtractorService sequenceExtractorService)
        {
            _largeNumberExtractionService = largeNumberExtractionService;
            _beneficialReplacementService = beneficialReplacementService;
            _sequenceExtractorService = sequenceExtractorService;
        }

        public async Task ProccesInputSequence(ISequence sequence, ProcessingOption option, IOutputDispatcherService sequenceWriter, string inputName, CancellationToken token)
        {
            var tasks = option.ExtractionOptions.Select(async extractionOption =>
            {
                var result = ProccesSequenceForOption(sequence, extractionOption.SelectOption);
                await SaveResult(sequenceWriter, result, inputName, extractionOption.SaveOptions, token);
            });

            await Task.WhenAll(tasks);
        }

        private async Task SaveResult(IOutputDispatcherService sequenceWriter, ISequence sequence, string sourceName, SaveToTextFileParam saveOptions, CancellationToken token)
        {
            if (saveOptions != null)
            {
                var request = new WriteRequest()
                {
                    Sequence = sequence,
                    SourceName = sourceName,
                    SaveTo = saveOptions,
                    OnWriteComplete = (s) => { (s as IPooledSequence)?.Release(); }
                };
                await sequenceWriter.AppendSequenceAsync(request, token);
            }
        }

        private ISequence ProccesSequenceForOption(ISequence sequence, IExtraction extractionOption)
        {
            switch (extractionOption)
            {
                case LargeNumberExtraction extractionOptions:
                    return _largeNumberExtractionService.ExtractLargeNumbers(sequence, extractionOptions);
                case ValueTransformation valueTransformationOptions:
                    return _beneficialReplacementService.PerformBeneficialReplacement(sequence, valueTransformationOptions);
                case SubSequenceExtraction extractionOptions:
                    return _sequenceExtractorService.ExstractSequence(sequence, extractionOptions);
                default:
                    throw new NotImplementedException(extractionOption.GetType().Name);
            }
        }
    }
}
