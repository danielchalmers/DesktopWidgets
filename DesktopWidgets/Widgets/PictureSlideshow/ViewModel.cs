using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly Random _random;
        private string _imageUrl;
        private int _index;

        public ViewModel(WidgetId id) : base(id)
        {
            Settings = id.GetSettings() as Settings;
            if (Settings == null)
                return;
            _filePathList = new List<string>();
            _random = new Random();

            _changeTimer = new DispatcherTimer {Interval = Settings.ChangeInterval};
            _changeTimer.Tick += (sender, args) => NextImage();

            UpdateFileList(false, false);
            NextImage();
            if (Settings.Recursive)
                UpdateFileList(Settings.Recursive, true);

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

        private void UpdateFileList(bool recursive, bool async)
        {
            if (string.IsNullOrWhiteSpace(Settings.RootPath) || !Directory.Exists(Settings.RootPath))
                return;
            Action getFiles = delegate
            {
                var filters = Settings.FileFilterExtension.Split('|');
                _filePathList.Clear();
                foreach (
                    var file in
                        Directory.EnumerateFiles(Settings.RootPath, "*.*",
                            recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
                {
                    var fileInfo = new FileInfo(file);
                    if (!filters.Contains(fileInfo.Extension.ToLower()) || fileInfo.Length > Settings.FileFilterSize)
                        continue;
                    _filePathList.Add(file);
                }
            };
            if (async)
                new Task(getFiles).Start();
            else
                getFiles();
        }

        private void NextImage()
        {
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