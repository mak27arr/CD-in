using CD_in_Core.Application.Services.Interfaces;
using CD_in_Core.Application.Services.Interfaces.Sequences;
using CD_in_Core.Application.Settings;
using CD_in_Core.Application.Settings.Input;
using CD_in_Core.Domain.Models.Sequences;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CD_in_Core.Application.Services
{
    internal class DirectoryProcessingService : IDirectoryProcessingService
    {
        private readonly IDeltaIndexProcessorService _deltaReader;
        private readonly ISequenceProcessingService _processingService;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DirectoryProcessingService> _logger;

        public DirectoryProcessingService(IDeltaIndexProcessorService deltaReader,
            ISequenceProcessingService sequenceProcessingService,
            IServiceProvider serviceProvider,
            ILogger<DirectoryProcessingService> logger)
        {
            _deltaReader = deltaReader;
            _processingService = sequenceProcessingService;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task ProcessDirectory(ProcessingOption option, Action<double> progressCallback, CancellationToken token)
        {
            var directorySettings = ValidateOption(option);
            var inputFilesPaths = Directory.GetFiles(directorySettings.FolderPath, directorySettings.InputFilesType);
            var fileCount = inputFilesPaths.Length;
            var sequence = new Sequence(option.DeltaParam.BlockSize);

            using var scope = _serviceProvider.CreateScope();
            var sequenceWriter = _serviceProvider.GetRequiredService<IOutputDispatcherService>();
            var offset = 0;

            foreach (var filePath in inputFilesPaths)
            {
                _logger.LogInformation("Start process file: {0}", filePath);
                var fileName = Path.GetFileNameWithoutExtension(filePath);
                var deltaResult = _deltaReader.ProcessFile(filePath, option.DeltaParam, (progress) => UpdateProgress(progress, fileCount, offset, progressCallback), token);
                sequence.Clear();

                await foreach (var element in deltaResult)
                {
                    if (token.IsCancellationRequested)
                        return;

                    sequence.Add(element);

                    if (sequence.Count == option.DeltaParam.BlockSize)
                    {
                        await _processingService.ProccesInputSequence(option, sequenceWriter, fileName, sequence, token);
                        sequence.Clear();
                    }
                }

                if (!token.IsCancellationRequested && sequence.Count > 0)
                {
                    await _processingService.ProccesInputSequence(option, sequenceWriter, fileName, sequence, token);
                }

                offset += 100 / fileCount;

                _logger.LogInformation("Finish process file: {0}", filePath);
            }

            await sequenceWriter.WaitToFinishAsync();
        }

        private static DirectoryInputSourceSettings ValidateOption(ProcessingOption option)
        {
            if (option.InputSource is not DirectoryInputSourceSettings directorySettings)
                throw new ArgumentException($"Not suported {option.InputSource.GetType().Name}");

            if (string.IsNullOrWhiteSpace(directorySettings.FolderPath))
                throw new ArgumentException("Please set FolderPath");

            if (string.IsNullOrWhiteSpace(directorySettings.InputFilesType))
                throw new ArgumentException("Please set InputFilesType");

            return directorySettings;
        }

        private void UpdateProgress(double progress, int fileCount, double offset, Action<double> progressCallback)
        {
            progressCallback?.Invoke((progress / fileCount) + offset);
        }
    }
}
