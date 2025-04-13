using CD_in_Core.Application.Services.Interfaces;
using CD_in_Core.Domain.Models;
using CD_in_Core.Domain.Models.Replacement;
using CD_in_Core.Domain.Models.Sequences;
using CD_in_Core.Infrastructure.FileServices;
using System.Threading;

namespace CD_in_Core.Application.Services
{
    public class MainProcessingService
    {
        private IDeltaIndexProcessorService _fileReader;
        private ILargeNumberExtractionService _largeNumberExtractionService;
        private IBeneficialReplacementService _beneficialReplacementService;
        private ISequenceExtractorService _sequenceExtractorService;
        private ISequenceWriter _sequenceWriter;

        public MainProcessingService(IDeltaIndexProcessorService fileReader, 
            ILargeNumberExtractionService largeNumberExtractionService,
            IBeneficialReplacementService beneficialReplacementService,
            ISequenceExtractorService sequenceExtractorService,
            ISequenceWriter sequenceWriter)
        {
            _fileReader = fileReader;
            _largeNumberExtractionService = largeNumberExtractionService;
            _beneficialReplacementService = beneficialReplacementService;
            _sequenceExtractorService = sequenceExtractorService;
            _sequenceWriter = sequenceWriter;
        }

        public async Task ProccessFiles(ProcessingOption option, CancellationToken token)
        {
            var inputFilePaths = Directory.GetFiles(option.FolderPath, option.InputFilesType);

            foreach(var filePath in inputFilePaths)
            {
                var fileName = Path.GetFileNameWithoutExtension(filePath);
                var deltaResult = _fileReader.ProcessFile(filePath, option.DeltaParam, token);
                var sequence = new Sequence();

                await foreach(var element in deltaResult)
                {
                    sequence.Add(element.Index, element.Value);

                    if (sequence.Digits.Count == option.DeltaParam.BlockSize)
                    {
                        foreach (var extractionOption in option.ExtractionOptions)
                        {
                            var result = ProccesSequenceForOption(sequence, extractionOption.SelectOption);
                            await SaveResult(result, fileName, extractionOption.SaveOptions);
                        }

                        sequence.Clear();
                    }
                }

                if (sequence.Digits.Count > 0)
                {
                    foreach (var extractionOption in option.ExtractionOptions)
                    {
                        var result = ProccesSequenceForOption(sequence, extractionOption.SelectOption);
                        await SaveResult(result, fileName, extractionOption.SaveOptions);
                    }
                }
            }
        }

        private async Task SaveResult(Sequence sequence, string sourceName, SequenceSaveOptions saveOptions)
        {
           if (saveOptions != null)
                await _sequenceWriter.AppendSequenceAsync(sequence, sourceName, saveOptions);
        }

        private Sequence ProccesSequenceForOption(Sequence sequence, IOptions extractionOption)
        {
            switch (extractionOption)
            {
                case LargeNumberExtractionOptions extractionOptions:
                    return _largeNumberExtractionService.ExtractLargeNumbers(sequence, extractionOptions);
                case ValueTransformationOptions valueTransformationOptions:
                    return _beneficialReplacementService.PerformBeneficialReplacement(sequence, valueTransformationOptions);
                case SequenceExtractionOptions extractionOptions:
                    return _sequenceExtractorService.ExstractSequence(sequence, extractionOptions);
                default:
                    throw new NotImplementedException(extractionOption.GetType().Name);
            }    
        }
    }
}
