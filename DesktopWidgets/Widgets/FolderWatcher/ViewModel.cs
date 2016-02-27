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
        private string _currentFileContent;
        private string _currentFilePath;
        private BitmapImage _currentImage;
        private DirectoryWatcher _directoryWatcher;
        private FileType _fileType = FileType.None;
        private bool _isShowing;

        public ViewModel(WidgetId guid) : base(guid)
        {
            Settings = guid.GetSettings() as Settings;
            if (Settings == null)
                return;

            IsPaused = Settings.Paused;
            CurrentFilePath = Settings.CurrentFilePath;

            OpenFile = new RelayCommand(OpenFileExecute);
            TogglePlayPause = new RelayCommand(TogglePlayPauseExecute);
            Next = new RelayCommand(NextExecute);
            Previous = new RelayCommand(PreviousExecute);

            if (Settings.ResumeWaitDuration.TotalSeconds > 0)
            {
                _resumeTimer = new DispatcherTimer {Interval = Settings.ResumeWaitDuration};
                _resumeTimer.Tick += (sender, args) => { Unpause(); };
            }

            _directoryWatcher =
                new DirectoryWatcher(Settings.DirectoryWatcherSettings, AddToFileQueue);
            _directoryWatcher.Start();

            CheckFile(false);
        }

        public ICommand OpenFile { get; set; }
        public ICommand TogglePlayPause { get; set; }
        public ICommand Next { get; set; }
        public ICommand Previous { get; set; }

        public Settings Settings { get; }

        public string CurrentFilePath
        {
            get { return _currentFilePath; }
            set
            {
                _currentFilePath = value;
                Settings.CurrentFilePath = value;
                RaisePropertyChanged(nameof(CurrentFilePath));
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
        public bool NextEnabled => HistoryIndex < Settings.History.Count - 1;

        private int HistoryIndex => Settings.History.IndexOf(CurrentFilePath);

        private void UpdateNextPrevious()
        {
            RaisePropertyChanged(nameof(PreviousEnabled));
            RaisePropertyChanged(nameof(NextEnabled));
        }

        private void AddToFileQueue(List<FileInfo> paths, DirectoryChange change)
        {
            Settings.History.AddRange(paths.Select(x => x.FullName));
            if (Settings.MaxHistory > 0 && Settings.History.Count > Settings.MaxHistory)
                Settings.History.RemoveRange(0, Settings.History.Count - Settings.MaxHistory);
            UpdateNextPrevious();
            RaisePropertyChanged(nameof(Settings.History));
            if (!Settings.QueueFiles || !_isShowing)
                HandleDirectoryChange();
        }

        private void CheckFile(bool playMedia = true)
        {
            if (string.IsNullOrWhiteSpace(Settings.CurrentFilePath) || !File.Exists(Settings.CurrentFilePath))
                return;
            if (HandleFileImage())
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
            if (HistoryIndex == Settings.History.Count - 1)
            {
                View?.HideUi();
                return;
            }
            _isShowing = true;
            CurrentFilePath = Settings.History[HistoryIndex + 1];
            CurrentFileContent = "";

            CheckFile();

            OnSpecialEvent();
        }

        private bool HandleFileImage()
        {
            if (Settings.ShowImages && ImageHelper.IsSupported(Path.GetExtension(CurrentFilePath)))
            {
                UpdateImage(CurrentFilePath);
                return true;
            }
            return false;
        }

        private bool HandleFileContent()
        {
            var isContent = Settings.ShowTextContentWhitelist != null && Settings.ShowTextContentWhitelist.Count > 0 &&
                            Settings.ShowTextContentWhitelist.Any(
                                x => x.EndsWith(Path.GetExtension(CurrentFilePath), StringComparison.OrdinalIgnoreCase)) &&
                            (Settings.ShowContentMaxSize <= 0 ||
                             new FileInfo(CurrentFilePath).Length <= Settings.ShowContentMaxSize);
            if (isContent)
                new Task(() => { CurrentFileContent = File.ReadAllText(CurrentFilePath); }).Start();
            return isContent;
        }

        private bool HandleFileMedia(bool play)
        {
            if (Settings.ShowImages && MediaPlayerHelper.IsSupported(Path.GetExtension(CurrentFilePath)))
            {
                if (play)
                    MediaPlayerStore.PlaySoundAsync(CurrentFilePath, Settings.PlayMediaVolume);
                return true;
            }
            return false;
        }

        public override void OnIntroEnd()
        {
            base.OnIntroEnd();
            _isShowing = false;
            HandleDirectoryChange();
        }

        public override void OnDismiss()
        {
            base.OnDismiss();
            _isShowing = false;
            HandleDirectoryChange();
        }

        private void UpdateImage(string imagePath)
        {
            var bmi = new BitmapImage();
            bmi.BeginInit();
            bmi.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            bmi.CacheOption = BitmapCacheOption.OnLoad;
            bmi.UriSource = new Uri(imagePath, UriKind.RelativeOrAbsolute);
            bmi.EndInit();

            CurrentImage = bmi;
        }

        private void OpenFileExecute()
        {
            ProcessHelper.Launch(CurrentFilePath);
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
            IsPaused = true;
        }

        private void Unpause()
        {
            _resumeTimer?.Stop();
            IsPaused = false;
            _isShowing = false;
            HandleDirectoryChange();
        }

        private void NextExecute()
        {
            _resumeTimer?.Stop();
            _resumeTimer?.Start();
            IsPaused = true;
            if (NextEnabled)
            {
                CurrentFilePath = Settings.History[HistoryIndex + 1];
                CheckFile();
            }
            UpdateNextPrevious();
        }

        private void PreviousExecute()
        {
            _resumeTimer?.Stop();
            _resumeTimer?.Start();
            IsPaused = true;
            if (PreviousEnabled)
            {
                CurrentFilePath = Settings.History[HistoryIndex - 1];
                CheckFile();
            }
            UpdateNextPrevious();
        }
    }
}