using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using DesktopWidgets.Classes;
using DesktopWidgets.Helpers;
using DesktopWidgets.Stores;
using DesktopWidgets.WidgetBase;
using DesktopWidgets.WidgetBase.ViewModel;
using GalaSoft.MvvmLight.CommandWpf;

namespace DesktopWidgets.Widgets.FolderWatcher
{
    public class ViewModel : WidgetViewModelBase
    {
        private readonly DispatcherTimer _resumeTimer;
        private FileInfo _currentFile;
        private string _currentFileContent;
        private BitmapImage _currentImage;
        private DirectoryWatcher _directoryWatcher;
        private FileType _fileType = FileType.None;
        private bool _isShowing;

        public ViewModel(WidgetId guid) : base(guid)
        {
            Settings = guid.GetSettings() as Settings;
            if (Settings == null)
                return;

            switch (Settings.ResumeOnStartMode)
            {
                case ResumeOnStartMode.Auto:
                    if (Settings.ResumeOnNextStart)
                    {
                        Settings.Paused = false;
                        Settings.ResumeOnNextStart = false;
                    }
                    break;
                case ResumeOnStartMode.Resume:
                    Settings.Paused = false;
                    break;
            }
            IsPaused = Settings.Paused;
            CurrentFile = Settings.CurrentFile;

            OpenFile = new RelayCommand(OpenFileExecute);
            TogglePlayPause = new RelayCommand(TogglePlayPauseExecute);
            Next = new RelayCommand(NextExecute);
            Previous = new RelayCommand(PreviousExecute);

            _resumeTimer = new DispatcherTimer();
            _resumeTimer.Tick += (sender, args) => { Unpause(); };

            _directoryWatcher = new DirectoryWatcher(Settings.DirectoryWatcherSettings, AddToFileQueue);
            _directoryWatcher.Start();

            CheckFile(false);
        }

        public ICommand OpenFile { get; set; }
        public ICommand TogglePlayPause { get; set; }
        public ICommand Next { get; set; }
        public ICommand Previous { get; set; }

        public Settings Settings { get; }

        public FileInfo CurrentFile
        {
            get { return _currentFile; }
            set
            {
                _currentFile = value;
                Settings.CurrentFile = value;
                RaisePropertyChanged(nameof(CurrentFile));
            }
        }

        public string CurrentFileContent
        {
            get { return _currentFileContent; }
            set
            {
                _currentFileContent = value;
                RaisePropertyChanged(nameof(CurrentFileContent));
            }
        }

        public BitmapImage CurrentImage
        {
            get { return _currentImage; }
            set
            {
                if (!Equals(_currentImage, value))
                {
                    _currentImage = value;
                    RaisePropertyChanged(nameof(CurrentImage));
                }
            }
        }

        public FileType FileType
        {
            get { return _fileType; }
            set
            {
                if (_fileType != value)
                {
                    _fileType = value;
                    RaisePropertyChanged(nameof(FileType));
                }
            }
        }

        public bool IsPaused
        {
            get { return Settings.Paused; }
            set
            {
                Settings.Paused = value;
                RaisePropertyChanged(nameof(IsPaused));
            }
        }

        public bool PreviousEnabled => HistoryIndex > 0;
        public bool NextEnabled => HistoryIndex < Settings.FileHistory.Count - 1;

        private int HistoryIndex => Settings.FileHistory.IndexOf(CurrentFile);

        private void UpdateNextPrevious()
        {
            RaisePropertyChanged(nameof(PreviousEnabled));
            RaisePropertyChanged(nameof(NextEnabled));
        }

        private void AddToFileQueue(List<FileInfo> paths, DirectoryChange change)
        {
            Settings.FileHistory.AddRange(paths);
            if (Settings.MaxFileHistory > 0 && Settings.FileHistory.Count > Settings.MaxFileHistory)
                Settings.FileHistory.RemoveRange(0, Settings.FileHistory.Count - Settings.MaxFileHistory);
            UpdateNextPrevious();
            RaisePropertyChanged(nameof(Settings.FileHistory));
            if (!Settings.QueueFiles || !_isShowing)
                HandleDirectoryChange();
        }

        private void CheckFile(bool playMedia = true)
        {
            if (Settings.CurrentFile == null || !File.Exists(Settings.CurrentFile.FullName))
                FileType = FileType.None;
            else if (HandleFileImage())
                FileType = FileType.Image;
            else if (HandleFileMedia(playMedia))
                FileType = FileType.Audio;
            else if (HandleFileContent())
                FileType = FileType.Text;
            else
            {
                FileType = FileType.Other;
            }
        }

        private void HandleDirectoryChange()
        {
            if (IsPaused)
                return;
            if (HistoryIndex == Settings.FileHistory.Count - 1)
            {
                View?.HideUi();
                return;
            }
            _isShowing = true;
            CurrentFile = Settings.FileHistory[HistoryIndex + 1];
            CurrentFileContent = "";

            CheckFile();

            OnSpecialEvent();
        }

        private bool HandleFileImage()
        {
            if (Settings.ShowImages && ImageHelper.IsSupported(CurrentFile.Extension))
            {
                UpdateImage();
                return true;
            }
            return false;
        }

        private bool HandleFileContent()
        {
            var isContent = Settings.ShowTextContentWhitelist != null && Settings.ShowTextContentWhitelist.Count > 0 &&
                            Settings.ShowTextContentWhitelist.Any(
                                x => x.EndsWith(CurrentFile.Extension, StringComparison.OrdinalIgnoreCase)) &&
                            (Settings.ShowContentMaxSize <= 0 || CurrentFile.Length <= Settings.ShowContentMaxSize);
            if (isContent)
                new Task(() => { CurrentFileContent = File.ReadAllText(CurrentFile.FullName); }).Start();
            return isContent;
        }

        private bool HandleFileMedia(bool play)
        {
            if (Settings.ShowImages && MediaPlayerHelper.IsSupported(CurrentFile.Extension))
            {
                if (play)
                    MediaPlayerStore.PlaySoundAsync(CurrentFile.FullName, Settings.PlayMediaVolume);
                return true;
            }
            return false;
        }

        private void UpdateImage()
        {
            var bmi = new BitmapImage();
            bmi.BeginInit();
            bmi.CacheOption = BitmapCacheOption.OnLoad;
            bmi.UriSource = new Uri(CurrentFile.FullName, UriKind.RelativeOrAbsolute);
            bmi.EndInit();

            CurrentImage = bmi;
        }

        private void OpenFileExecute()
        {
            ProcessHelper.Launch(CurrentFile.FullName);
            View?.Dismiss();
        }

        public override void OnClose()
        {
            base.OnClose();
            _directoryWatcher.Stop();
            _directoryWatcher = null;
        }

        public override void OnRefresh()
        {
            base.OnRefresh();
            _directoryWatcher.SetSettings(Settings.DirectoryWatcherSettings);
            _resumeTimer.Interval = Settings.ResumeWaitDuration;
        }

        private void TogglePlayPauseExecute()
        {
            if (IsPaused)
                Unpause();
            else
                Pause();
        }

        private void Pause()
        {
            _resumeTimer?.Stop();
            Settings.ResumeOnNextStart = false;
            IsPaused = true;
        }

        private void Unpause()
        {
            _resumeTimer?.Stop();
            Settings.ResumeOnNextStart = false;
            IsPaused = false;
        }

        private void PauseAndAutoResume()
        {
            if (Settings.ResumeWaitDuration.TotalSeconds > 0)
            {
                _resumeTimer?.Stop();
                _resumeTimer?.Start();
                if (!IsPaused)
                    Settings.ResumeOnNextStart = true;
                IsPaused = true;
            }
        }

        private void NextExecute()
        {
            PauseAndAutoResume();
            if (NextEnabled)
            {
                CurrentFile = Settings.FileHistory[HistoryIndex + 1];
                CheckFile();
            }
            UpdateNextPrevious();
        }

        private void PreviousExecute()
        {
            PauseAndAutoResume();
            if (PreviousEnabled)
            {
                CurrentFile = Settings.FileHistory[HistoryIndex - 1];
                CheckFile();
            }
            UpdateNextPrevious();
        }

        public override void ExecuteSpecialAction()
        {
            base.ExecuteSpecialAction();
            _isShowing = false;
            HandleDirectoryChange();
        }
    }
}