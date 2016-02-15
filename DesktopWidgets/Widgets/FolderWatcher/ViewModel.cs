using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
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
        private readonly Queue<string> _notificationQueue;
        private string _currentFileContent;
        private BitmapImage _currentImage;
        private DirectoryWatcher _directoryWatcher;
        private FileType _fileType = FileType.None;

        private int _historyIndex;
        private bool _isShowing;

        public ViewModel(WidgetId guid) : base(guid)
        {
            Settings = guid.GetSettings() as Settings;
            if (Settings == null)
                return;

            IsPaused = Settings.Paused;

            OpenFile = new RelayCommand(OpenFileExecute);
            TogglePlayPause = new RelayCommand(TogglePlayPauseExecute);
            Next = new RelayCommand(NextExecute);
            Previous = new RelayCommand(PreviousExecute);

            _notificationQueue = new Queue<string>();
            FileHistory = new ObservableCollection<string>();

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
            get { return Settings.CurrentFilePath; }
            set
            {
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

        public ObservableCollection<string> FileHistory { get; set; }

        public int HistoryIndex
        {
            get { return _historyIndex; }
            set
            {
                if (_historyIndex != value)
                {
                    _historyIndex = value;
                    RaisePropertyChanged(nameof(HistoryIndex));
                }
            }
        }

        private void AddToFileQueue(List<FileInfo> paths, DirectoryChange change)
        {
            var lastCheck = Settings.LastCheck;
            Settings.LastCheck = DateTime.Now;
            if (Settings.EnableTimeout)
                if (DateTime.Now - lastCheck >= Settings.TimeoutDuration)
                    return;
            var notificationCount = _notificationQueue.Count;
            foreach (var path in paths)
            {
                _notificationQueue.Enqueue(path.FullName);
                FileHistory.Add(path.FullName);
            }
            if (!Settings.QueueFiles || (!_isShowing && notificationCount == 0))
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
            if (_notificationQueue.Count == 0)
            {
                View?.HideUi();
                return;
            }
            _isShowing = true;
            var nextFile = _notificationQueue.Dequeue();
            HistoryIndex = FileHistory.IndexOf(nextFile);
            CurrentFilePath = nextFile;
            CurrentFileContent = "";

            CheckFile(true);

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
            _notificationQueue.Clear();
            IsPaused = !IsPaused;
        }

        private void NextExecute()
        {
            if (Settings.PauseOnSwitch)
                IsPaused = true;
            _notificationQueue.Clear();
            if (HistoryIndex < FileHistory.Count - 1)
            {
                HistoryIndex++;
                CurrentFilePath = FileHistory[HistoryIndex];
            }
        }

        private void PreviousExecute()
        {
            if (Settings.PauseOnSwitch)
                IsPaused = true;
            _notificationQueue.Clear();
            if (HistoryIndex > 0)
            {
                HistoryIndex--;
                CurrentFilePath = FileHistory[HistoryIndex];
            }
        }
    }
}