using System;
using System.Collections.Generic;
using System.IO;
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

        private readonly List<string> _supportedImageExtensions = new List<string>
        {
            ".bmp",
            ".gif",
            ".ico",
            ".jpg",
            ".jpeg",
            ".png",
            ".tiff"
        };

        private string _currentFilePath;

        private BitmapImage _currentImage;
        private DirectoryWatcher _directoryWatcher;

        private bool _isImage;

        private bool _isShowing;

        public ViewModel(WidgetId guid) : base(guid)
        {
            Settings = guid.GetSettings() as Settings;
            if (Settings == null)
                return;

            IsImage = false;

            OpenFile = new RelayCommand(OpenFileExecute);

            RefreshAction = delegate { _directoryWatcher.SetWatchPath(Settings.WatchFolder); };

            _notificationQueue = new Queue<string>();
            _directoryWatcher =
                new DirectoryWatcher(
                    new DirectoryWatcherSettings
                    {
                        WatchFolder = Settings.WatchFolder,
                        IncludeFilter = Settings.IncludeFilter,
                        ExcludeFilter = Settings.ExcludeFilter,
                        Recursive = Settings.Recursive,
                        CheckInterval = TimeSpan.FromMilliseconds(Settings.FolderCheckIntervalMS)
                    }, AddToFileQueue);
            _directoryWatcher.Start();
        }

        public ICommand OpenFile { get; set; }

        public Settings Settings { get; }

        public bool IsImage
        {
            get { return _isImage; }
            set
            {
                if (_isImage != value)
                {
                    _isImage = value;
                    RaisePropertyChanged(nameof(IsImage));
                }
            }
        }

        public string CurrentFilePath
        {
            get { return _currentFilePath; }
            set
            {
                if (_currentFilePath != value)
                {
                    _currentFilePath = value;
                    RaisePropertyChanged(nameof(CurrentFilePath));
                }
            }
        }

        public BitmapImage CurrentImage
        {
            get { return _currentImage; }
            set
            {
                if (_currentImage != value)
                {
                    _currentImage = value;
                    RaisePropertyChanged(nameof(CurrentImage));
                }
            }
        }

        private void AddToFileQueue(FileInfo path, DirectoryChange change)
        {
            if (change == DirectoryChange.FileChanged && !Settings.ShowModifiedFiles)
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

        private void HandleDirectoryChange()
        {
            if (_notificationQueue.Count == 0)
            {
                Hide();
                return;
            }
            _isShowing = true;
            //if (Settings.ReplaceExistingFile || _notificationQueue.Count > 0)
            //{
            CurrentFilePath = _notificationQueue.Dequeue();

            if (_supportedImageExtensions.Contains(Path.GetExtension(CurrentFilePath).ToLower()))
            {
                UpdateImage(CurrentFilePath);
                IsImage = true;
            }
            else
            {
                IsImage = false;
            }

            if (!App.IsMuted)
                MediaPlayerStore.PlaySoundAsync(Settings.EventSoundPath, Settings.EventSoundVolume);
            if (Settings.OpenOnEvent)
                Settings.Identifier.GetView()
                    .ShowIntro(Settings.OpenOnEventStay ? 0 : (int) Settings.OpenOnEventDuration.TotalMilliseconds,
                        false, false, false);
            //}
        }

        public override void OnIntroFinish()
        {
            base.OnIntroFinish();
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
            Hide();
        }

        private void Hide()
        {
            Settings.Identifier.GetView().HideUI();
        }

        public override void OnClose()
        {
            base.OnClose();
            _directoryWatcher.Stop();
            _directoryWatcher = null;
        }
    }
}