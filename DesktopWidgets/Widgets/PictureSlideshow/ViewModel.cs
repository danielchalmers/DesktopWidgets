using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Threading;
using DesktopWidgets.Classes;
using DesktopWidgets.Helpers;
using DesktopWidgets.ViewModelBase;

namespace DesktopWidgets.Widgets.PictureSlideshow
{
    public class ViewModel : WidgetViewModelBase
    {
        private readonly DispatcherTimer _changeTimer;

        private readonly List<string> _filePathList;
        private string _imageUrl;
        private int _index;
        private readonly Random _random;

        public ViewModel(WidgetId id) : base(id)
        {
            Settings = id.GetSettings() as Settings;
            if (Settings == null)
                return;
            _filePathList = new List<string>();
            _random = new Random();
            _changeTimer = new DispatcherTimer();
            _changeTimer.Interval = Settings.ChangeInterval;
            _changeTimer.Tick += (sender, args) => NextImage();
            _changeTimer.Start();
            NextImage();
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

        private void UpdateFileList()
        {
            if (string.IsNullOrWhiteSpace(Settings.RootPath) || !Directory.Exists(Settings.RootPath))
                return;
            var filters = Settings.FileFilterExtension.Split('|');
            _filePathList.Clear();
            foreach (
                var file in
                    Directory.EnumerateFiles(Settings.RootPath, "*.*",
                        Settings.Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
            {
                var fileInfo = new FileInfo(file);
                if (!filters.Contains(fileInfo.Extension.ToLower()) || fileInfo.Length > Settings.FileFilterSize)
                    continue;
                _filePathList.Add(file);
            }
        }

        private void NextImage()
        {
            UpdateFileList();
            if (_filePathList.Count == 0)
                return;
            string newImagePath;

            if (Settings.Shuffle)
            {
                newImagePath = _filePathList[_random.Next(0, _filePathList.Count)];
            }
            else
            {
                if (_index > _filePathList.Count - 1)
                    _index = 0;
                newImagePath = _filePathList[_index];
                _index++;
            }

            ImageUrl = newImagePath;
        }
    }
}