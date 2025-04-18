using CD_in_Core.Application.Services.Interfaces;
using CD_in_Core.Domain.Models.DeltaIndex;
using CD_in_Core.Domain.Models.Replacement;
using CD_in_Core.Domain.Models.Sequences;
using CD_in_Core.Domain.Models.Spec;
using CD_in_Core.Domain.Models;
using CommunityToolkit.Mvvm.Input;
using System.IO;
using System.Windows.Threading;
using System.Diagnostics;
using CD_in.ViewModel;
using System.Windows;
using Microsoft.Extensions.Logging;
using System.Windows.Controls;

namespace CD_in
{
    public partial class MainViewModel : BaseViewModel
    {
        private DispatcherTimer _timer;
        private Stopwatch _stopwatch;
        private readonly ILogger<MainViewModel> _logger;
        private IFolderProcessingService _folderProcessingService;
        private CancellationTokenSource? _tokenSource;

        #region ICommand

        public RelayCommand ProcessFolderCommand { get; }
        public RelayCommand CancelCommand { get; }
        public RelayCommand CDInCommand { get; set; }

        #endregion

        public MainViewModel(ILogger<MainViewModel> logger, IFolderProcessingService folderProcessingService)
        {
            _logger = logger;
            _folderProcessingService = folderProcessingService;
            _stopwatch = new Stopwatch();
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(100)
            };
            _timer.Tick += (s, e) => UpdateExlapsedTime();

            ProcessFolderCommand = new RelayCommand(async () => await ExecuteAsync(), () => CanProcessFolder);
            CancelCommand = new RelayCommand(Cancel);
            CDInCommand = new RelayCommand(OnCDInButtonClicked);
        }

        #region Command handler

        private async Task ExecuteAsync()
        {
            try
            {
                IsExecuting = true;
                _tokenSource = new CancellationTokenSource();
                ResetAndStartMeasurement();
                var option = BuildProcessingOption();
                await Task.Run(() => _folderProcessingService.ProcessFolderAsync(option, UpdateProgress, _tokenSource.Token));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Processing Failed");
                MessageBox.Show($"Error: {ex.Message}", "Processing Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                _tokenSource?.Dispose();
                _tokenSource = null;
                StopMeasurement();
                IsExecuting = false;
            }
        }

        private void Cancel()
        {
            _tokenSource?.Cancel();
        }

        private void OnCDInButtonClicked()
        {
            Microsoft.Win32.OpenFolderDialog dialog = new();
            dialog.Multiselect = false;
            dialog.Title = "Select a folder";

            if (dialog.ShowDialog() == true)
            {
                CDInFolderPath = dialog.FolderName;
            }
        }

        #endregion

        #region Helper

        private void UpdateProgress(double progress)
        {
            Progress = progress;
        }

        private void ResetAndStartMeasurement()
        {
            UpdateProgress(0);
            _timer.Start();
            _stopwatch.Reset();
            _stopwatch.Start();
        }

        private void StopMeasurement()
        {
            _stopwatch.Stop();
            _timer.Stop();
            UpdateExlapsedTime();
        }

        private ProcessingOption BuildProcessingOption()
        {
            var saveFolder = GetFolderForSafe(CDInFolderPath);
            Directory.CreateDirectory(saveFolder);

            var option = new ProcessingOption()
            {
                FolderPath = CDInFolderPath,
                DeltaParam = new DeltaIndexParams()
                {
                    BlockSize = BlockSize,
                },
                ExtractionOptions = new List<ExtractionOptions>(),
                InputFilesType = "*.txt"
            };

            if (IsMergeChecked)
            {
                option.ExtractionOptions.Add(new ExtractionOptions()
                {
                    ExecutionOrder = MergeOrderExecution,
                    SelectOption = new SubSequenceExtractionOptions()
                    {
                        Condition = new EqualsCondition(1),
                        MinSequenceLength = MergeOrderLength
                    },
                    SaveOptions = new SequenceSaveOptions()
                    {
                        FileName = "Обєднання 1",
                        FilePath = saveFolder
                    }
                });
            }

            if (IsMergeSecondChecked)
            {
                option.ExtractionOptions.Add(new ExtractionOptions()
                {
                    ExecutionOrder = MergeSecondOrderExecution,
                    SelectOption = new SubSequenceExtractionOptions()
                    {
                        Condition = new EqualsCondition(2),
                        MinSequenceLength = MergeSecondOrderLength
                    },
                    SaveOptions = new SequenceSaveOptions()
                    {
                        FileName = "Обєднання 2",
                        FilePath = saveFolder
                    }
                });
            }

            if (IsReplaceChecked)
            {
                option.ExtractionOptions.Add(new ExtractionOptions()
                {
                    ExecutionOrder = ReplaceOrderExecution,
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
                });
            }

            if (IsLargerChecked)
            {
                option.ExtractionOptions.Add(new ExtractionOptions()
                {
                    ExecutionOrder = LargerOrderExecution,
                    SelectOption = new LargeNumberExtractionOptions()
                    {
                        Condition = new GreaterOrEqualThanCondition(LargerNumberValue)
                    },
                    SaveOptions = new SequenceSaveOptions()
                    {
                        FileName = "Виніс великих",
                        FilePath = saveFolder
                    }
                });
            }

            return option;
        }

        private static string GetFolderForSafe(string? folderPath)
        {
            var parentFolder = Path.GetDirectoryName(folderPath);
            return parentFolder != null ? Path.Combine(parentFolder, "CD-out") : string.Empty;
        }

        private void UpdateExlapsedTime()
        {
            ElapsedTime = _stopwatch?.Elapsed ?? TimeSpan.MinValue;
        }

        #endregion
    }
}
