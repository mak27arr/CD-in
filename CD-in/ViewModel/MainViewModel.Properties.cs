using System.IO;

namespace CD_in
{
    public partial class MainViewModel
    {
        #region variables

        private bool _isExecuting;
        private double _progress;
        private int _blockSize = 100000;
        private bool _isStatisticsChecked;
        private bool _isWithoutCdOutChecked;
        private string? _inFolderPath;
        private bool _isMergeChecked = true;
        private bool _isMergeSecondChecked = true;
        private bool _isReplaceChecked = true;
        private bool _isLargerChecked = true;
        private int _largerNumberValue = 11;
        private int _mergeOrderLength = 11;
        private int _mergeSecondOrderLength = 6;
        private int _mergeOrderExecution = 1;
        private int _mergeSecondOrderExecution = 2;
        private int _replaceOrderExecution = 3;
        private int _largerOrderExecution = 4;
        private TimeSpan _elapsedTime;

        #endregion

        public override string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case nameof(BlockSize):
                        return BlockSize <= 0 ? "Розмір блоку має бути позитивним" : string.Empty;
                    case nameof(MergeOrderLength):
                        return IsMergeChecked && MergeOrderLength <= 0 ? "Довжина об'єднання має бути позитивною" : string.Empty;
                    case nameof(MergeSecondOrderLength):
                        return IsMergeSecondChecked && MergeSecondOrderLength <= 0 ? "Довжина об'єднання має бути позитивною" : string.Empty;
                    case nameof(LargerNumberValue):
                        return IsLargerChecked && LargerNumberValue <= 0 ? "Значення для виносу має бути позитивним" : string.Empty;
                    default:
                        return string.Empty;
                }
            }
        }

        private bool CanProcessFolder => 
            !IsExecuting 
            && !string.IsNullOrWhiteSpace(CDInFolderPath) 
            && Directory.Exists(CDInFolderPath) 
            && IsPropertysValid(nameof(BlockSize), nameof(MergeOrderLength), nameof(MergeSecondOrderLength), nameof(LargerNumberValue));

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
    }
}
