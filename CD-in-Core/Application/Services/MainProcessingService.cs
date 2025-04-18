using CD_in_Core.Application.Services.Interfaces;
using CD_in_Core.Domain.Models;
using CD_in_Core.Domain.Models.Replacement;
using CD_in_Core.Domain.Models.Sequences;
using CD_in_Core.Infrastructure.FileServices.Interfaces;
using CD_in_Core.Infrastructure.FileServices.Writer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CD_in_Core.Application.Services
{
    internal class MainProcessingService : IMainProcessingService
    {
        private readonly IDeltaIndexProcessorService _fileReader;
        private readonly ILargeNumberExtractionService _largeNumberExtractionService;
        private readonly IBeneficialReplacementService _beneficialReplacementService;
        private readonly ISubSequenceExtractorService _sequenceExtractorService;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<MainProcessingService> _logger;

        public MainProcessingService(IDeltaIndexProcessorService fileReader,
            ILargeNumberExtractionService largeNumberExtractionService,
            IBeneficialReplacementService beneficialReplacementService,
            ISubSequenceExtractorService sequenceExtractorService,
            IServiceProvider serviceProvider,
            ILogger<MainProcessingService> logger)
        {
            _fileReader = fileReader;
            _largeNumberExtractionService = largeNumberExtractionService;
            _beneficialReplacementService = beneficialReplacementService;
            _sequenceExtractorService = sequenceExtractorService;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task ProcessFiles(ProcessingOption option, Action<double> progressCallback, CancellationToken token)
        {
            if (option == null)
                return;

            if (string.IsNullOrWhiteSpace(option.FolderPath))
                throw new ArgumentException("Please set FolderPath");

            if (string.IsNullOrWhiteSpace(option.InputFilesType))
                throw new ArgumentException("Please set InputFilesType");

            var inputFilesPaths = Directory.GetFiles(option.FolderPath, option.InputFilesType);
            var fileCount = inputFilesPaths.Count();
            var sequenceWriter = _serviceProvider.GetRequiredService<ISequenceWriter>();

            foreach (var filePath in inputFilesPaths)
            {
                _logger.LogInformation("Start process file: {0}", filePath);
                var fileName = Path.GetFileNameWithoutExtension(filePath);
                var deltaResult = _fileReader.ProcessFile(filePath, option.DeltaParam, (progress) => UpdateProgress(progress, fileCount, progressCallback), token);
                var sequence = new Sequence(option.DeltaParam.BlockSize);

                await foreach (var element in deltaResult)
                {
                    if (token.IsCancellationRequested)
                        return;

                    sequence.Add(element);

                    if (sequence.Count == option.DeltaParam.BlockSize)
                    {
                        await ProccesInputSequence(option, sequenceWriter, fileName, sequence, token);
                        sequence.Clear();
                    }
                }

                if (!token.IsCancellationRequested && sequence.Count > 0)
                {
                    await ProccesInputSequence(option, sequenceWriter, fileName, sequence, token);
                }

                _logger.LogInformation("Finish process file: {0}", filePath);
            }

            await sequenceWriter.WaitToFinishAsync();
        }

        private async Task ProccesInputSequence(ProcessingOption option, ISequenceWriter sequenceWriter, string fileName, ISequence sequence, CancellationToken token)
        {
            var tasks = option.ExtractionOptions.Select(async extractionOption =>
            {
                var result = ProccesSequenceForOption(sequence, extractionOption.SelectOption);
                await SaveResult(sequenceWriter, result, fileName, extractionOption.SaveOptions, token);
            });

            await Task.WhenAll(tasks);
        }

        private void UpdateProgress(double progress, int fileCount, Action<double> progressCallback)
        {
            progressCallback?.Invoke(progress / fileCount);
        }

        private async Task SaveResult(ISequenceWriter sequenceWriter, ISequence sequence, string sourceName, SequenceSaveOptions saveOptions, CancellationToken token)
        {
            if (saveOptions != null)
            {
                var requwest = new WriteRequest()
                {
                    Sequence = sequence,
                    SourceFileName = sourceName,
                    Options = saveOptions,
                    OnWriteComplete = (s) => { (s as IPooledSequence)?.Release(); }
                };
                await sequenceWriter.AppendSequenceAsync(requwest, token);
            }
        }

        private ISequence ProccesSequenceForOption(ISequence sequence, IOptions extractionOption)
        {
            switch (extractionOption)
            {
                case LargeNumberExtractionOptions extractionOptions:
                    return _largeNumberExtractionService.ExtractLargeNumbers(sequence, extractionOptions);
                case ValueTransformationOptions valueTransformationOptions:
                    return _beneficialReplacementService.PerformBeneficialReplacement(sequence, valueTransformationOptions);
                case SubSequenceExtractionOptions extractionOptions:
                    return _sequenceExtractorService.ExstractSequence(sequence, extractionOptions);
                default:
                    throw new NotImplementedException(extractionOption.GetType().Name);
            }
        }
    }
}
