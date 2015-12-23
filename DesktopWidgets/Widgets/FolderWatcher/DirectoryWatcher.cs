using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Threading;

namespace DesktopWidgets.Widgets.FolderWatcher
{
    internal class DirectoryWatcher
    {
        private readonly DispatcherTimer _dirWatcherTimer;
        private readonly Dictionary<string, IEnumerable<string>> _knownFilePaths;
        private readonly Action<string> _newFileAction;
        private readonly Settings _settings;

        public DirectoryWatcher(Settings settings, Action<string> newFileAction)
        {
            _settings = settings;
            _newFileAction = newFileAction;
            _knownFilePaths = new Dictionary<string, IEnumerable<string>>();
            _dirWatcherTimer = new DispatcherTimer
            {
                Interval = _settings.FolderCheckInterval
            };
            _dirWatcherTimer.Tick += (sender, args) => CheckDirectoryForNewFiles(true);

            CheckDirectoryForNewFiles(false);
        }

        private void CheckDirectoryForNewFiles(bool promptAction)
        {
            try
            {
                var folder = _settings.WatchFolder;
                if (!_knownFilePaths.ContainsKey(folder))
                    _knownFilePaths.Add(folder, null);
                var exclude = _settings.ExcludeFilter.Split('|');
                if (string.IsNullOrWhiteSpace(_settings.IncludeFilter))
                    _settings.IncludeFilter = "*.*";
                var files = Directory.GetFiles(folder, _settings.IncludeFilter);
                foreach (var file in files)
                {
                    if (_knownFilePaths[folder] == null || _knownFilePaths[folder].Any(x => x == file))
                        continue;
                    if (exclude.Any(x => x.EndsWith(Path.GetExtension(file), StringComparison.OrdinalIgnoreCase)))
                        continue;
                    if (promptAction)
                        _newFileAction(file);
                }
                _knownFilePaths[folder] = files;
            }
            catch
            {
                // ignored
            }
        }

        public void Start()
        {
            _dirWatcherTimer.Start();
        }

        public void Stop()
        {
            _dirWatcherTimer.Stop();
        }
    }
}