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
        private bool _isScanning;

        public DirectoryWatcher(DirectoryWatcherSettings settings,
            Action<FileInfo, DirectoryChange> newFileAction = null)
        {
            _settings = settings;
            _newFileAction = newFileAction;
            KnownFilePaths = new Dictionary<string, List<FileInfo>>();
            _dirWatcherTimer = new DispatcherTimer {Interval = _settings.CheckInterval};
            _dirWatcherTimer.Tick += (sender, args) => CheckDirectoriesForNewFilesAsync();
        }

        public void CheckDirectoriesForNewFilesAsync(bool promptAction = true)
            => new Task(() => { CheckDirectoriesForNewFiles(promptAction); }).Start();

        public void CheckDirectoriesForNewFiles(bool promptAction = true)
        {
            if (_isScanning)
                return;
            foreach (var folder in _settings.WatchFolders)
            {
                if (string.IsNullOrWhiteSpace(folder))
                    continue;
                var dirInfo = new DirectoryInfo(folder);
                if (!dirInfo.Exists)
                    continue;
                _isScanning = true;
                try
                {
                    if (!KnownFilePaths.ContainsKey(folder))
                        KnownFilePaths.Add(folder, null);
                    var files = dirInfo.EnumerateFiles("*.*",
                        _settings.Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
                        .AsParallel()
                        .ToList();
                    foreach (var file in files)
                    {
                        if (KnownFilePaths[folder] == null)
                            break;
                        if (file.Length > _settings.MaxSize && _settings.MaxSize > 0)
                            continue;
                        if (_settings.FileExtensionWhitelist != null && _settings.FileExtensionWhitelist.Count > 0 &&
                            !_settings.FileExtensionWhitelist.Any(
                                x => x.EndsWith(file.Extension, StringComparison.OrdinalIgnoreCase)))
                            continue;
                        if (_settings.FileExtensionBlacklist != null && _settings.FileExtensionBlacklist.Count > 0 &&
                            _settings.FileExtensionBlacklist.Any(
                                x => x.EndsWith(file.Extension, StringComparison.OrdinalIgnoreCase)))
                            continue;
                        if (promptAction)
                        {
                            var sameFiles =
                                KnownFilePaths[folder].Where(x => x.FullName == file.FullName).ToList();
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
                    KnownFilePaths[folder] = files;
                }
                catch
                {
                    // ignored
                }
            }
            _isScanning = false;
        }

        public void SetWatchPaths(List<string> paths)
        {
            _settings.WatchFolders = paths.ToList();
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