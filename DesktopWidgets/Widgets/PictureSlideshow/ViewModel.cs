using System;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using DesktopWidgets.Classes;
using DesktopWidgets.Helpers;
using DesktopWidgets.WidgetBase;
using DesktopWidgets.WidgetBase.ViewModel;

namespace DesktopWidgets.Widgets.PictureSlideshow
{
    public class ViewModel : WidgetViewModelBase
    {
        private readonly Random _random;
        private DispatcherTimer _changeTimer;
        private DirectoryWatcher _directoryWatcher;
        private string _imageUrl;
        private int _index;

        public ViewModel(WidgetId id) : base(id)
        {
            Settings = id.GetSettings() as Settings;
            if (Settings == null)
                return;
            AllowDrop = Settings.AllowDropFiles;
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
                    Settings.ImageUrl = value;
                    RaisePropertyChanged(nameof(ImageUrl));
                }
            }
        }

        private void NextImage()
        {
            if (Settings.Freeze || string.IsNullOrWhiteSpace(Settings.RootPath) ||
                !_directoryWatcher.KnownFilePaths.ContainsKey(Settings.RootPath) ||
                _directoryWatcher.KnownFilePaths[Settings.RootPath] == null)
                return;
            var paths = _directoryWatcher.KnownFilePaths[Settings.RootPath];
            string newImagePath;

            if (Settings.Shuffle)
            {
                newImagePath =
                    paths[
                        _random.Next(0, paths.Count)].FullName;
            }
            else
            {
                if (_index > _directoryWatcher.KnownFilePaths.Count - 1)
                    _index = 0;
                newImagePath = paths[_index].FullName;
                _index++;
            }

            ImageUrl = newImagePath;
        }

        public override void OnClose()
        {
            base.OnClose();
            _directoryWatcher?.Stop();
            _directoryWatcher = null;
            _changeTimer?.Stop();
            _changeTimer = null;
        }

        public override void DropExecute(DragEventArgs e)
        {
            if (AllowDrop && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                ImageUrl = ((string[]) e.Data.GetData(DataFormats.FileDrop)).FirstOrDefault();
                Settings.Freeze = true;
            }
        }

        public override void OnRefresh()
        {
            base.OnRefresh();
            _directoryWatcher.SetWatchPath(Settings.RootPath);
        }
    }
}