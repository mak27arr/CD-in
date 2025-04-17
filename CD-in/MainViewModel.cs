using CD_in_Core.Application.Services.Interfaces;
using CD_in_Core.Domain.Models.DeltaIndex;
using CD_in_Core.Domain.Models.Replacement;
using CD_in_Core.Domain.Models.Sequences;
using CD_in_Core.Domain.Models.Spec;
using CD_in_Core.Domain.Models;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.IO;
using System.Windows.Threading;
using System.Diagnostics;

namespace CD_in
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region variables

        private DispatcherTimer _timer;
        private Stopwatch _stopwatch;
        private IFolderProcessingService _folderProcessingService;
        private CancellationTokenSource? tokenSource;
        private bool _isExecuting;
        private double _progress;
        private int _blockSize = 100000;
        private bool _isStatisticsChecked;
        private bool _isWithoutCdOutChecked;
        private string? _inFolderPath;
        private bool _isMergeChecked;
        private bool _isMergeSecondChecked;
        private bool _isReplaceChecked;
        private bool _isLargerChecked;
        private int _largerNumberValue = 11;
        private int _mergeOrderLength = 11;
        private int _mergeSecondOrderLength = 6;
        private int _mergeOrderExecution = 1;
        private int _mergeSecondOrderExecution = 2;
        private int _replaceOrderExecution = 3;
        private int _largerOrderExecution = 4;
        private TimeSpan _elapsedTime;

        #endregion

        public int BlockSize
        {
            get => _blockSize;
            set
            {
                _blockSize = value;
                OnPropertyChanged();
                ProcessFolderCommand.NotifyCanExecuteChanged();
            }
        }

        public bool IsStatisticsChecked
        {
            get => _isStatisticsChecked;
            set
            {
                _isStatisticsChecked = value;
                OnPropertyChanged();
            }
        }

        public bool IsWithoutCdOutChecked
        {
            get => _isWithoutCdOutChecked;
            set
            {
                _isWithoutCdOutChecked = value;
                OnPropertyChanged();
            }
        }

        public string? CDInFolderPath
        {
            get => _inFolderPath;
            set
            {
                _inFolderPath = value;
                OnPropertyChanged();
                ProcessFolderCommand.NotifyCanExecuteChanged();
            }
        }

        public bool IsExecuting
        {
            get => _isExecuting;
            set
            {
                _isExecuting = value;
                OnPropertyChanged(nameof(IsExecuting));
                ProcessFolderCommand.NotifyCanExecuteChanged();
            }
        }

        public double Progress
        {
            get => _progress;
            set
            {
                _progress = value;
                OnPropertyChanged(nameof(Progress));
            }
        }

        public TimeSpan ElapsedTime
        {
            get => _elapsedTime;
            set
            {
                _elapsedTime = value;
                OnPropertyChanged();
            }
        }

        #region Option

        public bool IsMergeChecked
        {
            get => _isMergeChecked;
            set
            {
                _isMergeChecked = value;
                OnPropertyChanged();
            }
        }

        public bool IsMergeSecondChecked
        {
            get => _isMergeSecondChecked;
            set
            {
                _isMergeSecondChecked = value;
                OnPropertyChanged();
            }
        }

        public bool IsReplaceChecked
        {
            get => _isReplaceChecked;
            set
            {
                _isReplaceChecked = value;
                OnPropertyChanged();
            }
        }

        public bool IsLargerChecked
        {
            get => _isLargerChecked;
            set
            {
                _isLargerChecked = value;
                OnPropertyChanged();
            }
        }

        public int MergeOrderExecution
        {
            get => _mergeOrderExecution;
            set
            {
                _mergeOrderExecution = value;
                OnPropertyChanged();
            }
        }

        public int MergeSecondOrderExecution
        {
            get => _mergeSecondOrderExecution;
            set
            {
                _mergeSecondOrderExecution = value;
                OnPropertyChanged();
            }
        }

        public int ReplaceOrderExecution
        {
            get => _replaceOrderExecution;
            set
            {
                _replaceOrderExecution = value;
                OnPropertyChanged();
            }
        }

        public int LargerNumberValue
        {
            get => _largerNumberValue;
            set
            {
                _largerNumberValue = value;
                OnPropertyChanged();
            }
        }

        public int LargerOrderExecution
        {
            get => _largerOrderExecution;
            set
            {
                _largerOrderExecution = value;
                OnPropertyChanged();
            }
        }

        public int MergeOrderLength
        {
            get => _mergeOrderLength;
            set
            {
                _mergeOrderLength = value;
                OnPropertyChanged();
            }
        }

        public int MergeSecondOrderLength
        {
            get => _mergeSecondOrderLength;
            set
            {
                _mergeSecondOrderLength = value;
                OnPropertyChanged();
            }
        }

        #endregion

        private bool CanProcessFolder => !IsExecuting && !string.IsNullOrWhiteSpace(CDInFolderPath) && Directory.Exists(CDInFolderPath) && BlockSize > 0;

        #region ICommand

        public RelayCommand ProcessFolderCommand { get; }
        public RelayCommand CancelCommand { get; }
        public RelayCommand CDInCommand { get; set; }

        #endregion

        public MainViewModel(IFolderProcessingService folderProcessingService)
        {
            _folderProcessingService = folderProcessingService;
            _stopwatch = new Stopwatch();
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(50)
            };
            _timer.Tick += (s, e) => ElapsedTime = _stopwatch?.Elapsed ?? TimeSpan.MinValue;

            ProcessFolderCommand = new RelayCommand(async () => await ExecuteAsync(), () => CanProcessFolder);
            CancelCommand = new RelayCommand(Cancel);
            CDInCommand = new RelayCommand(OnCDInButtonClicked);
        }

        #region Command handler

        private async Task ExecuteAsync()
        {
            IsExecuting = true;
            tokenSource = new CancellationTokenSource();
            ResetAndStartMeasurement();
            var option = BuildProcessingOption();
            await Task.Run(() => _folderProcessingService.ProcessFolderAsync(option, UpdateProgress, tokenSource.Token));
            StopMeasurement();
            IsExecuting = false;
        }

        private void StopMeasurement()
        {
            _stopwatch.Stop();
            _timer.Stop();
        }

        private void ResetAndStartMeasurement()
        {
            UpdateProgress(0);
            _timer.Start();
            _stopwatch.Reset();
            _stopwatch.Start();
        }

        private void Cancel()
        {
            tokenSource?.Cancel();
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

        private void UpdateProgress(double progress)
        {
            Progress = progress;
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

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
