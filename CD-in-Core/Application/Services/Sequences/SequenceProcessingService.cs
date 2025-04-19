using CD_in_Core.Application.Services.Interfaces.Sequences;
using CD_in_Core.Application.Services.IO;
using CD_in_Core.Application.Settings;
using CD_in_Core.Domain.Models.Sequences;
using CD_in_Core.Domain.Select;
using CD_in_Core.Domain.ValueObjects;
using CD_in_Core.Infrastructure;

namespace CD_in_Core.Application.Services.Sequences
{
    internal class SequenceProcessingService : ISequenceProcessingService
    {
        private readonly ISequenceExtractionService _numberExtractionService;
        private readonly IReplacementService _beneficialReplacementService;
        private readonly ISubSequenceExtractorService _sequenceExtractorService;

        public SequenceProcessingService(
            ISequenceExtractionService largeNumberExtractionService,
            IReplacementService beneficialReplacementService,
            ISubSequenceExtractorService sequenceExtractorService)
        {
            _numberExtractionService = largeNumberExtractionService;
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

        private ISequence ProccesSequenceForOption(ISequence sequence, IExtractionRule extractionOption)
        {
            switch (extractionOption)
            {
                case SelectNumberRule extractionOptions:
                    return _numberExtractionService.Extract(sequence, extractionOptions);
                case ValueTransformationRule valueTransformationOptions:
                    return _beneficialReplacementService.PerformReplacement(sequence, valueTransformationOptions);
                case SubSequenceExtractionRule extractionOptions:
                    return _sequenceExtractorService.ExstractSubSequence(sequence, extractionOptions);
                case RawSequenceExtractionRules extractionRules:
                    return _numberExtractionService.CloneSequence(sequence);
                default:
                    throw new NotImplementedException($"Cannot process for {extractionOption.GetType().Name}");
            }
        }
    }
}
