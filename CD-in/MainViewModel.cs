using CD_in_Core.Application.Services;
using GalaSoft.MvvmLight.CommandWpf;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;

namespace CD_in
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region variables

        private string _blockSize = "100000";
        private bool _isStatisticsChecked;
        private bool _isWithoutCdOutChecked;
        private string? _inFolderPath;

        #endregion

        public string BlockSize
        {
            get => _blockSize;
            set
            {
                _blockSize = value;
                OnPropertyChanged();
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
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public bool IsExecuting
        {
            get => _isExecuting;
            set
            {
                _isExecuting = value;
                OnPropertyChanged(nameof(IsExecuting));
            }
        }

        #region ICommand

        public ICommand ExecuteCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand CDInCommand { get; set; }

        #endregion

        private FolderProcessingService _folderProcessingService;
        private CancellationTokenSource? tokenSource;
        private bool _isExecuting;

        public MainViewModel(FolderProcessingService folderProcessingService)
        {
            _folderProcessingService = folderProcessingService;
            ExecuteCommand = new RelayCommand(async () => await ExecuteAsync(), CanExecute);
            CancelCommand = new RelayCommand(Cancel);
            CDInCommand = new RelayCommand(OnCDInButtonClicked);
        }

        #region Command handler

        private async Task ExecuteAsync()
        {
            IsExecuting = true;
            tokenSource = new CancellationTokenSource();
            if (int.TryParse(BlockSize, out var blockSize))
                await _folderProcessingService.ProcessFolderAsync(CDInFolderPath, blockSize, tokenSource.Token);

            IsExecuting = false;
        }

        private bool CanExecute()
        {
            return !IsExecuting && !string.IsNullOrWhiteSpace(CDInFolderPath) && Directory.Exists(CDInFolderPath);
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
                CDInFolderPath = dialog.FolderName;
        }

        #endregion

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
