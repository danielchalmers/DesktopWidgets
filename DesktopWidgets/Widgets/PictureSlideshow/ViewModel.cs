using System;
using System.Windows.Threading;
using DesktopWidgets.Classes;
using DesktopWidgets.Helpers;
using DesktopWidgets.ViewModelBase;

namespace DesktopWidgets.Widgets.PictureSlideshow
{
    public class ViewModel : WidgetViewModelBase
    {
        private readonly DispatcherTimer _changeTimer;
        private readonly DirectoryWatcher _directoryWatcher;

        private readonly Random _random;
        private string _imageUrl;
        private int _index;

        public ViewModel(WidgetId id) : base(id)
        {
            Settings = id.GetSettings() as Settings;
            if (Settings == null)
                return;
            _random = new Random();

            _changeTimer = new DispatcherTimer {Interval = Settings.ChangeInterval};
            _changeTimer.Tick += (sender, args) => NextImage();

            _directoryWatcher = new DirectoryWatcher(new DirectoryWatcherSettings
            {
                WatchFolder = Settings.RootPath,
                IncludeFilter = Settings.FileFilterExtension,
                MaxSize = Settings.FileFilterSize,
                Recursive = Settings.Recursive
            });
            _directoryWatcher.CheckDirectoryForNewFiles();
            NextImage();
            if (Settings.Recursive)
                _directoryWatcher.CheckDirectoryForNewFilesAsync();

            _changeTimer.Start();
        }

        public Settings Settings { get; }

        public string ImageUrl
        {
            get { return _imageUrl; }
            set
            {
                if (_imageUrl != value)
                {
                    _imageUrl = value;
                    RaisePropertyChanged(nameof(ImageUrl));
                }
            }
        }

        private void NextImage()
        {
            if (string.IsNullOrWhiteSpace(Settings.RootPath) ||
                !_directoryWatcher.KnownFilePaths.ContainsKey(Settings.RootPath) ||
                _directoryWatcher.KnownFilePaths[Settings.RootPath] == null)
                return;
            string newImagePath;

            if (Settings.Shuffle)
            {
                newImagePath =
                    _directoryWatcher.KnownFilePaths[Settings.RootPath][
                        _random.Next(0, _directoryWatcher.KnownFilePaths.Count)];
            }
            else
            {
                if (_index > _directoryWatcher.KnownFilePaths.Count - 1)
                    _index = 0;
                newImagePath = _directoryWatcher.KnownFilePaths[Settings.RootPath][_index];
                _index++;
            }

            ImageUrl = newImagePath;
        }
    }
}