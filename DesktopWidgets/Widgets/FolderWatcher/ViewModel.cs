using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using DesktopWidgets.Classes;
using DesktopWidgets.Helpers;
using DesktopWidgets.ViewModelBase;
using GalaSoft.MvvmLight.CommandWpf;

namespace DesktopWidgets.Widgets.FolderWatcher
{
    public class ViewModel : WidgetViewModelBase
    {
        private readonly DirectoryWatcher _directoryWatcher;
        private readonly DispatcherTimer _hideTimer;
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

        private bool _isImage;

        public ViewModel(WidgetId guid) : base(guid)
        {
            Settings = guid.GetSettings() as Settings;
            if (Settings == null)
                return;

            IsImage = false;

            OpenFile = new RelayCommand(OpenFileExecute);
            Dismiss = new RelayCommand(DismissExecute);
            Mute = new RelayCommand(MuteExecute);

            _notificationQueue = new Queue<string>();
            _directoryWatcher = new DirectoryWatcher(Settings, AddToFileQueue);
            _directoryWatcher.Start();
        }

        public ICommand OpenFile { get; set; }
        public ICommand Dismiss { get; set; }
        public ICommand Mute { get; set; }

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

        private void AddToFileQueue(string path)
        {
            var lastCheck = Settings.LastCheck;
            Settings.LastCheck = DateTime.Now;
            if (Settings.EnableTimeout)
                if (DateTime.Now - lastCheck >= Settings.TimeoutDuration)
                    return;
            _notificationQueue.Enqueue(path);
            HandleDirectoryChange();
        }

        private void HandleDirectoryChange()
        {
            //if (Settings.NotificationReplaceExisting || _notificationQueue.Count > 0)
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

            if (Settings.MuteEndTime < DateTime.Now)
            {
                Show();
                MediaPlayerStore.PlaySoundAsync(Settings.EventSoundPath, Settings.EventSoundVolume);
            }
            //}
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

        private void MuteExecute()
        {
            if (Settings.MuteEndTime > DateTime.Now)
            {
                Settings.MuteEndTime = DateTime.Now;
            }
            else
            {
                Hide();
                Settings.MuteEndTime = DateTime.Now + Settings.MuteDuration;
            }
        }

        private void DismissExecute()
        {
            Hide();
        }

        private void Show()
        {
            if (Settings.OpenOnEvent)
                Settings.Identifier.GetView()
                    .ShowIntro(Settings.OpenOnEventStay ? 0 : (int) Settings.OpenOnEventDuration.TotalMilliseconds,
                        false);
        }

        private void Hide()
        {
            Settings.Identifier.GetView().HideUI();
        }
    }
}