using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace DesktopWidgets.Classes
{
    internal class DirectoryWatcher
    {
        private readonly DispatcherTimer _dirWatcherTimer;
        private readonly Action<FileInfo, DirectoryChange> _newFileAction;
        private readonly DirectoryWatcherSettings _settings;
        public readonly Dictionary<string, List<FileInfo>> KnownFilePaths;
        private IEnumerable<string> _excludeFilter;

        private IEnumerable<string> _includeFilter;
        private bool _isScanning;

        public DirectoryWatcher(DirectoryWatcherSettings settings,
            Action<FileInfo, DirectoryChange> newFileAction = null)
        {
            _settings = settings;
            _newFileAction = newFileAction;
            KnownFilePaths = new Dictionary<string, List<FileInfo>>();
            _dirWatcherTimer = new DispatcherTimer {Interval = _settings.CheckInterval};
            _dirWatcherTimer.Tick += (sender, args) => CheckDirectoryForNewFilesAsync();

            SetFilters(_settings.IncludeFilter, _settings.ExcludeFilter);
        }

        public void SetFilters(string includeFilter, string excludeFilter)
        {
            _includeFilter = !string.IsNullOrWhiteSpace(includeFilter)
                ? _settings.IncludeFilter.Split('|')
                : null;
            _excludeFilter = !string.IsNullOrWhiteSpace(excludeFilter)
                ? _settings.ExcludeFilter.Split('|')
                : null;
        }

        public void CheckDirectoryForNewFilesAsync(bool promptAction = true)
            => new Task(() => { CheckDirectoryForNewFiles(promptAction); }).Start();

        public void CheckDirectoryForNewFiles(bool promptAction = true)
        {
            if (_isScanning || string.IsNullOrWhiteSpace(_settings.WatchFolder))
                return;
            var dirInfo = new DirectoryInfo(_settings.WatchFolder);
            if (!dirInfo.Exists)
                return;
            _isScanning = true;
            try
            {
                if (!KnownFilePaths.ContainsKey(_settings.WatchFolder))
                    KnownFilePaths.Add(_settings.WatchFolder, null);
                var files = dirInfo.EnumerateFiles("*.*",
                    _settings.Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
                    .AsParallel()
                    .ToList();
                foreach (var file in files)
                {
                    if (KnownFilePaths[_settings.WatchFolder] == null)
                        break;
                    if (file.Length > _settings.MaxSize && _settings.MaxSize > 0)
                        continue;
                    if (_settings.IncludeFilter != "*.*" && _includeFilter != null &&
                        !_includeFilter.Any(x => x.EndsWith(file.Extension, StringComparison.OrdinalIgnoreCase)))
                        continue;
                    if (_excludeFilter != null &&
                        _excludeFilter.Any(x => x.EndsWith(file.Extension, StringComparison.OrdinalIgnoreCase)))
                        continue;
                    if (promptAction)
                    {
                        var sameFiles =
                            KnownFilePaths[_settings.WatchFolder].Where(x => x.FullName == file.FullName).ToList();
                        if (sameFiles.Count > 0)
                        {
                            if (sameFiles.Any(x => x.LastWriteTimeUtc != file.LastWriteTimeUtc))
                            {
                                Application.Current.Dispatcher.Invoke(
                                    () => { _newFileAction?.Invoke(file, DirectoryChange.FileChanged); });
                            }
                        }
                        else
                        {
                            Application.Current.Dispatcher.Invoke(
                                () => { _newFileAction?.Invoke(file, DirectoryChange.NewFile); });
                        }
                    }
                }
                KnownFilePaths[_settings.WatchFolder] = files;
            }
            catch
            {
                // ignored
            }
            _isScanning = false;
        }

        public bool SetWatchPath(string path)
        {
            if (_settings.WatchFolder != path)
            {
                _settings.WatchFolder = path;
                return true;
            }
            return false;
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