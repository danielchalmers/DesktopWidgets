using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using DesktopWidgets.Classes;
using DesktopWidgets.Helpers;
using DesktopWidgets.WidgetBase;
using DesktopWidgets.WidgetBase.ViewModel;
using GalaSoft.MvvmLight.CommandWpf;

namespace DesktopWidgets.Widgets.PictureSlideshow
{
    public class ViewModel : WidgetViewModelBase
    {
        private readonly Random _random;
        private DispatcherTimer _changeTimer;
        private DirectoryWatcher _directoryWatcher;
        private string _imageUrl;
        private int _index;
        private bool _isPaused;

        public ViewModel(WidgetId id) : base(id)
        {
            Settings = id.GetSettings() as Settings;
            if (Settings == null)
            {
                return;
            }
            IsPaused = Settings.Freeze;
            ImageUrl = Settings.ImageUrl;

            TogglePlayPause = new RelayCommand(TogglePlayPauseExecute);
            Drop = new RelayCommand<DragEventArgs>(DropExecute);

            _random = new Random();

            _changeTimer = new DispatcherTimer { Interval = Settings.ChangeInterval };
            _changeTimer.Tick += (sender, args) => NextImage();

            _directoryWatcher = new DirectoryWatcher(new DirectoryWatcherSettings
            {
                WatchFolders = new List<string> { Settings.RootPath },
                FileExtensionWhitelist = ImageHelper.SupportedExtensions,
                MaxSize = Settings.FileFilterSize,
                Recursive = Settings.Recursive,
                DetectNewFiles = false,
                DetectModifiedFiles = false
            });
            _directoryWatcher.CheckDirectoriesForNewFiles();
            NextImage();
            if (Settings.Recursive)
            {
                _directoryWatcher.CheckDirectoriesForNewFilesAsync();
            }

            _changeTimer.Start();
        }

        public Settings Settings { get; }
        public ICommand TogglePlayPause { get; set; }
        public ICommand Drop { get; set; }

        public string ImageUrl
        {
            get { return _imageUrl; }
            set
            {
                if (_imageUrl != value)
                {
                    _imageUrl = value;
                    Settings.ImageUrl = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsPaused
        {
            get { return _isPaused; }
            set
            {
                if (_isPaused != value)
                {
                    _isPaused = value;
                    Settings.Freeze = value;
                    RaisePropertyChanged();
                }
            }
        }

        private void NextImage()
        {
            if (Settings.Freeze || string.IsNullOrWhiteSpace(Settings.RootPath) ||
                !_directoryWatcher.KnownFilePaths.ContainsKey(Settings.RootPath) ||
                _directoryWatcher.KnownFilePaths[Settings.RootPath] == null)
            {
                return;
            }
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
                {
                    _index = 0;
                }
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

        public void DropExecute(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var imagePath = ((string[])e.Data.GetData(DataFormats.FileDrop))
                    .Where(path => ImageHelper.IsSupported(Path.GetExtension(path)))
                    .FirstOrDefault();
                if (imagePath != null)
                {
                    ImageUrl = imagePath;
                    IsPaused = true;
                }
            }
        }

        public override void OnRefresh()
        {
            base.OnRefresh();
            _changeTimer.Interval = Settings.ChangeInterval;
            _directoryWatcher.SetWatchPaths(new List<string> { Settings.RootPath });
        }

        private void TogglePlayPauseExecute()
        {
            IsPaused = !IsPaused;
        }
    }
}