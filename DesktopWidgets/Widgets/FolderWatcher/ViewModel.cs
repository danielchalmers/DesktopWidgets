using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        private bool _isShowing;

        public ViewModel(WidgetId guid) : base(guid)
        {
            Settings = guid.GetSettings() as Settings;
            if (Settings == null)
                return;

            OpenFile = new RelayCommand(OpenFileExecute);

            _notificationQueue = new Queue<string>();
            _directoryWatcher =
                new DirectoryWatcher(
                    new DirectoryWatcherSettings
                    {
                        WatchFolders = Settings.WatchFolders,
                        FileExtensionWhitelist = Settings.FileExtensionWhitelist,
                        FileExtensionBlacklist = Settings.FileExtensionBlacklist,
                        Recursive = Settings.Recursive,
                        CheckInterval = TimeSpan.FromMilliseconds(Settings.FolderCheckIntervalMS)
                    }, AddToFileQueue);
            _directoryWatcher.Start();

            CheckFile(false);
        }

        public ICommand OpenFile { get; set; }

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

        private void AddToFileQueue(FileInfo path, DirectoryChange change)
        {
            if (change == DirectoryChange.FileChanged && !Settings.DetectModifiedFiles)
                return;
            if (change == DirectoryChange.NewFile && !Settings.DetectNewFiles)
                return;
            var lastCheck = Settings.LastCheck;
            Settings.LastCheck = DateTime.Now;
            if (Settings.EnableTimeout)
                if (DateTime.Now - lastCheck >= Settings.TimeoutDuration)
                    return;
            _notificationQueue.Enqueue(path.FullName);
            if (!Settings.QueueFiles || (!_isShowing && _notificationQueue.Count == 1))
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
            if (_notificationQueue.Count == 0)
            {
                View?.HideUi();
                return;
            }
            _isShowing = true;
            CurrentFilePath = _notificationQueue.Dequeue();
            CurrentFileContent = "";

            CheckFile(true);

            if (Settings.OpenOnEvent)
            {
                View?.ShowIntro(new IntroData
                {
                    Duration = (int) Settings.OpenOnEventDuration.TotalMilliseconds,
                    HideOnFinish = false,
                    ExecuteFinishAction = !Settings.OpenOnEventStay,
                    SoundPath = Settings.EventSoundPath,
                    SoundVolume = Settings.EventSoundVolume
                });
            }
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
                            new FileInfo(CurrentFilePath).Length <= Settings.ShowContentMaxSize;
            if (isContent)
                CurrentFileContent = File.ReadAllText(CurrentFilePath);
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
            _directoryWatcher.SetWatchPaths(Settings.WatchFolders);
        }
    }
}