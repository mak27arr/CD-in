﻿using CD_in_Core.Application.Services.Interfaces;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.IO;

namespace CD_in
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region variables

        private IFolderProcessingService _folderProcessingService;
        private CancellationTokenSource? tokenSource;
        private bool _isExecuting;
        private double _progress;
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

        private bool CanProcessFolder => !IsExecuting && !string.IsNullOrWhiteSpace(CDInFolderPath) && Directory.Exists(CDInFolderPath);

        #region ICommand

        public RelayCommand ProcessFolderCommand { get; }
        public RelayCommand CancelCommand { get; }
        public RelayCommand CDInCommand { get; set; }

        #endregion

        public MainViewModel(IFolderProcessingService folderProcessingService)
        {
            _folderProcessingService = folderProcessingService;
            ProcessFolderCommand = new RelayCommand(async () => await ExecuteAsync(), () => CanProcessFolder);
            CancelCommand = new RelayCommand(Cancel);
            CDInCommand = new RelayCommand(OnCDInButtonClicked);
        }

        #region Command handler

        private async Task ExecuteAsync()
        {
            IsExecuting = true;
            tokenSource = new CancellationTokenSource();
            UpdateProgress(0);

            if (int.TryParse(BlockSize, out var blockSize))
                await Task.Run(() => _folderProcessingService.ProcessFolderAsync(CDInFolderPath, blockSize, UpdateProgress, tokenSource.Token));

            IsExecuting = false;
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

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
