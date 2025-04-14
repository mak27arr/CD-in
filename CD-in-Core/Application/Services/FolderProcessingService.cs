using CD_in_Core.Application.Services.Interfaces;
using CD_in_Core.Domain.Models;
using CD_in_Core.Domain.Models.DeltaIndex;
using CD_in_Core.Domain.Models.Replacement;
using CD_in_Core.Domain.Models.Sequences;
using CD_in_Core.Domain.Models.Spec;

namespace CD_in_Core.Application.Services
{
    internal class FolderProcessingService : IFolderProcessingService
    {
        private readonly IMainProcessingService _mainProcessingService;

        public FolderProcessingService(IMainProcessingService mainProcessingService)
        {
            _mainProcessingService = mainProcessingService;
        }

        public async Task ProcessFolderAsync(string? folderPath, int blockSize, Action<double> progressCallback, CancellationToken cancellationToken)
        {
            var processingOption = BuildProcessingOption(folderPath, blockSize);
            await _mainProcessingService.ProcessFiles(processingOption, progressCallback, cancellationToken);
        }

        private ProcessingOption BuildProcessingOption(string? folderPath, int blockSize)
        {
            var saveFolder = GetFolderForSafe(folderPath);
            Directory.CreateDirectory(saveFolder);
            return new ProcessingOption()
            {
                FolderPath = folderPath,
                DeltaParam = new DeltaIndexParams()
                {
                    BlockSize = blockSize,
                },
                ExtractionOptions = new List<ExtractionOptions>()
                {
                    new ExtractionOptions()
                    {
                        SelectOption = new SequenceExtractionOptions()
                        {
                            TargetDigit = 1,
                            MinSequenceLength = 11
                        },
                        SaveOptions = new SequenceSaveOptions()
                        {
                            FileName = "Обєднання 1",
                            FilePath = saveFolder
                        }
                    },
                    new ExtractionOptions()
                    {
                        SelectOption = new SequenceExtractionOptions()
                        {
                            TargetDigit = 2,
                            MinSequenceLength = 6
                        },
                        SaveOptions = new SequenceSaveOptions()
                        {
                            FileName = "Обєднання 2",
                            FilePath = saveFolder
                        }
                    },
                    new ExtractionOptions()
                    {
                        SelectOption = new ValueTransformationOptions()
                        {
                            Specification = new ReplaceSingleTwosWithOnesSpecification(),
                            ReplacementStrategy = new ConstantTransformer(1)
                        },
                        SaveOptions = new SequenceSaveOptions()
                        {
                            FileName = "Заміна",
                            FilePath = saveFolder
                        }
                    },
                    new ExtractionOptions()
                    {
                        SelectOption = new LargeNumberExtractionOptions()
                        {
                            Condition = new GreaterThanCondition(10)
                        },
                        SaveOptions = new SequenceSaveOptions()
                        {
                            FileName = "Виніс великих",
                            FilePath = saveFolder
                        }
                    },
                },
                InputFilesType = "*.txt"
            };
        }

        private static string GetFolderForSafe(string? folderPath)
        {
            var parentFolder = Path.GetDirectoryName(folderPath);
            return parentFolder != null ? Path.Combine(parentFolder, "CD-out") : string.Empty;
        }
    }
}
