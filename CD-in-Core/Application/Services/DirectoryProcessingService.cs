using CD_in_Core.Application.Services.Interfaces;
using CD_in_Core.Application.Services.Interfaces.Sequences;
using CD_in_Core.Application.Services.IO;
using CD_in_Core.Application.Settings;
using CD_in_Core.Application.Settings.Input;
using CD_in_Core.Domain.Models.Sequences;
using CD_in_Core.Infrastructure.FileServices.Reader;
using Microsoft.Extensions.Logging;

namespace CD_in_Core.Application.Services
{
    internal class DirectoryProcessingService : IDirectoryProcessingService
    {
        private readonly IInputDispatcherService _inputDispatcherService;
        private readonly ISequenceProcessingService _processingService;
        private readonly IOutputDispatcherFactory _outputDispatcherFactory;
        private readonly ILogger<DirectoryProcessingService> _logger;

        public DirectoryProcessingService(IDeltaIndexTextFileReader deltaReader,
            IInputDispatcherService inputDispatcherService,
            ISequenceProcessingService sequenceProcessingService,
            IOutputDispatcherFactory outputDispatcherFactory,
            ILogger<DirectoryProcessingService> logger)
        {
            _inputDispatcherService = inputDispatcherService;
            _processingService = sequenceProcessingService;
            _outputDispatcherFactory = outputDispatcherFactory;
            _logger = logger;
        }

        public async Task ProcessDirectory(ProcessingOption option, Action<double> progressCallback, CancellationToken token)
        {
            var directorySettings = ValidateOption(option);
            var inputFilesPaths = Directory.GetFiles(directorySettings.FolderPath, directorySettings.InputFilesType);
            var fileCount = inputFilesPaths.Length;
            var progressPerFile = 100d / fileCount;
            var sequence = new Sequence(option.DeltaParam.BlockSize);
            var outputDispatcher = _outputDispatcherFactory.Create();

            for (int fileIndex = 0; fileIndex < inputFilesPaths.Length; fileIndex++)
            {
                sequence.Clear();
                string? filePath = inputFilesPaths[fileIndex];
                _logger.LogInformation("Start process file: {0}", filePath);
                var fileName = Path.GetFileNameWithoutExtension(filePath);

                var fileReadParam = new TextFileSourceParam()
                {
                    Path = filePath,
                    BlockSize = option.DeltaParam.BlockSize,
                };

                var deltaResult = _inputDispatcherService.GetInputDelta(fileReadParam, (progress) => UpdateProgress(progress, fileIndex, progressPerFile, progressCallback), token);

                await foreach (var element in deltaResult)
                {
                    if (token.IsCancellationRequested)
                        return;

                    sequence.Add(element);

                    if (sequence.Count == option.DeltaParam.BlockSize)
                    {
                        await _processingService.ProccesInputSequence(sequence, option, outputDispatcher, fileName, token);
                        sequence.Clear();
                    }
                }

                if (!token.IsCancellationRequested && sequence.Count > 0)
                {
                    await _processingService.ProccesInputSequence(sequence, option, outputDispatcher, fileName, token);
                }

                _logger.LogInformation("Finish process file: {0}", filePath);
            }

            await outputDispatcher.WaitToFinishAsync();
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

        private void UpdateProgress(double progress, int fileIndex, double progressPerFile, Action<double> progressCallback)
        {
            progressCallback?.Invoke((progressPerFile * fileIndex) + ((progressPerFile / 100) * progress));
        }
    }
}
