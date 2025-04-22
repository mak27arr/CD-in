using CD_in_Core.Application.Services.Interfaces;
using CommunityToolkit.Mvvm.Input;
using System.IO;
using System.Windows.Threading;
using System.Diagnostics;
using CD_in.ViewModel;
using System.Windows;
using Microsoft.Extensions.Logging;
using CD_in_Core.Application.Settings;
using CD_in_Core.Application.Settings.DeltaIndex;
using CD_in_Core.Application.Settings.Input;
using CD_in_Core.Domain.Conditions;
using CD_in_Core.Domain.Select;
using CD_in_Core.Domain.ValueObjects;

namespace CD_in
{
    public partial class MainViewModel : BaseViewModel
    {
        private DispatcherTimer _timer;
        private Stopwatch _stopwatch;
        private readonly ILogger<MainViewModel> _logger;
        private IMainProcessingService _mainProcessingService;
        private CancellationTokenSource? _tokenSource;

        #region ICommand

        public RelayCommand ProcessFolderCommand { get; }
        public RelayCommand CancelCommand { get; }
        public RelayCommand CDInCommand { get; set; }

        #endregion

        public MainViewModel(ILogger<MainViewModel> logger, IMainProcessingService folderProcessingService)
        {
            _logger = logger;
            _mainProcessingService = folderProcessingService;
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
                await Task.Run(() => _mainProcessingService.ProcessAsync(option, UpdateProgress, _tokenSource.Token));
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
                InputSource = new DirectoryInputSourceSettings() { 
                    FolderPath = CDInFolderPath,
                    InputFilesType = "*.txt"
                },
                DeltaParam = new DeltaIndexParams()
                {
                    BlockSize = BlockSize,
                },
                ExtractionOptions = new List<ExtractionSettings>()
            };

            if (IsMergeChecked)
            {
                option.ExtractionOptions.Add(new ExtractionSettings()
                {
                    ExecutionOrder = MergeOrderExecution,
                    SelectOption = new SubSequenceExtractionRule()
                    {
                        Condition = new EqualsCondition(1),
                        MinSequenceLength = MergeOrderLength
                    },
                    SaveOptions = new SaveToTextFileParam()
                    {
                        FileName = "Обєднання 1",
                        FilePath = saveFolder
                    }
                });
            }

            if (IsMergeSecondChecked)
            {
                option.ExtractionOptions.Add(new ExtractionSettings()
                {
                    ExecutionOrder = MergeSecondOrderExecution,
                    SelectOption = new SubSequenceExtractionRule()
                    {
                        Condition = new EqualsCondition(2),
                        MinSequenceLength = MergeSecondOrderLength
                    },
                    SaveOptions = new SaveToTextFileParam()
                    {
                        FileName = "Обєднання 2",
                        FilePath = saveFolder
                    }
                });
            }

            if (IsReplaceChecked)
            {
                option.ExtractionOptions.Add(new ExtractionSettings()
                {
                    ExecutionOrder = ReplaceOrderExecution,
                    SelectOption = new ValueTransformationRule()
                    {
                        Specification = new ReplaceSingleTwosWithOnesSpecification(),
                        ReplacementStrategy = new ConstantTransformer(1)
                    },
                    SaveOptions = new SaveToTextFileParam()
                    {
                        FileName = "Заміна",
                        FilePath = saveFolder
                    }
                });
            }

            if (IsLargerChecked)
            {
                option.ExtractionOptions.Add(new ExtractionSettings()
                {
                    ExecutionOrder = LargerOrderExecution,
                    SelectOption = new SelectNumberRule()
                    {
                        Condition = new GreaterOrEqualThanCondition(LargerNumberValue)
                    },
                    SaveOptions = new SaveToTextFileParam()
                    {
                        FileName = "Виніс великих",
                        FilePath = saveFolder
                    }
                });
            }

            if (!option.ExtractionOptions.Any())
            {
                option.ExtractionOptions.Add(new ExtractionSettings()
                {
                    ExecutionOrder = LargerOrderExecution,
                    SelectOption = new RawSequenceExtractionRules(),
                    SaveOptions = new SaveToTextFileParam()
                    {
                        FileName = "Дельта індекси",
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
