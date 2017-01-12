using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace DesktopWidgets.Classes
{
    internal class DirectoryWatcher
    {
        private readonly DispatcherTimer _dirWatcherTimer;
        private readonly Dictionary<string, bool> _isScanningDictionary;
        private readonly Action<List<FileInfo>, DirectoryChange> _newFileAction;
        public readonly Dictionary<string, List<FileInfo>> KnownFilePaths;
        private DirectoryWatcherSettings _settings;

        public DirectoryWatcher(DirectoryWatcherSettings settings,
            Action<List<FileInfo>, DirectoryChange> newFileAction = null)
        {
            _settings = settings;
            _newFileAction = newFileAction;
            KnownFilePaths = new Dictionary<string, List<FileInfo>>();
            _isScanningDictionary = new Dictionary<string, bool>();
            _dirWatcherTimer = new DispatcherTimer();
            _dirWatcherTimer.Tick += (sender, args) => CheckDirectoriesForNewFiles();
            UpdateTimerInterval();
        }

        public void CheckDirectoriesForNewFilesAsync()
            => Task.Run(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                CheckDirectoriesForNewFiles();
            });

        public void CheckDirectoriesForNewFiles()
        {
            var lastCheck = _settings.LastCheck;
            _settings.LastCheck = DateTime.Now;
            if (_settings.TimeoutDuration.TotalSeconds > 0 && DateTime.Now - lastCheck >= _settings.TimeoutDuration)
            {
                return;
            }
            foreach (var folder in _settings.WatchFolders)
            {
                Task.Run(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    CheckDirectoryForNewFiles(folder);
                });
            }
        }

        private void CheckDirectoryForNewFiles(string folder)
        {
            if (string.IsNullOrWhiteSpace(folder))
            {
                return;
            }
            try
            {
                if (!_isScanningDictionary.ContainsKey(folder))
                {
                    _isScanningDictionary.Add(folder, false);
                }
                else if (_isScanningDictionary[folder])
                {
                    return;
                }
                _isScanningDictionary[folder] = true;

                if (!Directory.Exists(folder))
                {
                    _isScanningDictionary[folder] = false;
                    return;
                }

                var dirInfo = new DirectoryInfo(folder);
                if (!KnownFilePaths.ContainsKey(folder))
                {
                    KnownFilePaths.Add(folder, null);
                }
                var files = dirInfo.EnumerateFiles("*.*",
                    _settings.Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
                    .Where(IsFileLengthValid)
                    .Where(IsFileExtensionValid)
                    .ToList();

                var oldFiles = KnownFilePaths[folder]?.ToList();
                KnownFilePaths[folder] = files;

                if (oldFiles != null)
                {
                    if (_settings.DetectModifiedFiles)
                    {
                        var changedFiles =
                            files.Where(
                                x =>
                                    oldFiles.Any(
                                        y =>
                                            y.FullName == x.FullName &&
                                            y.LastWriteTimeUtc != x.LastWriteTimeUtc))
                                .OrderBy(x => x.LastWriteTimeUtc)
                                .ToList();
                        Application.Current.Dispatcher.Invoke(
                            () =>
                            {
                                try
                                {
                                    if (changedFiles.Count > 0)
                                    {
                                        _newFileAction?.Invoke(changedFiles, DirectoryChange.FileChanged);
                                    }
                                }
                                catch
                                {
                                    // ignored
                                }
                            });
                    }
                    if (_settings.DetectNewFiles)
                    {
                        var newFiles =
                            files.Where(x => oldFiles.All(y => y.FullName != x.FullName))
                                .OrderBy(x => x.LastWriteTimeUtc)
                                .ToList();
                        Application.Current.Dispatcher.Invoke(
                            () =>
                            {
                                try
                                {
                                    if (newFiles.Count > 0)
                                    {
                                        _newFileAction?.Invoke(newFiles, DirectoryChange.NewFile);
                                    }
                                }
                                catch
                                {
                                    // ignored
                                }
                            });
                    }
                }
            }
            catch
            {
                // ignored
            }
            finally
            {
                if (_isScanningDictionary.ContainsKey(folder))
                {
                    _isScanningDictionary[folder] = false;
                }
            }
        }

        private bool IsFileLengthValid(FileInfo file)
        {
            return _settings.MaxSize <= 0 || file.Length <= _settings.MaxSize;
        }

        private bool IsFileExtensionValid(FileInfo file)
        {
            return IsFileWhitelistValid(file) && IsFileBlacklistValid(file);
        }

        private bool IsFileWhitelistValid(FileInfo file)
        {
            return _settings.FileExtensionWhitelist == null || _settings.FileExtensionWhitelist.Count == 0 ||
                   _settings.FileExtensionWhitelist.Any(
                       x => x.EndsWith(file.Extension, StringComparison.OrdinalIgnoreCase));
        }

        private bool IsFileBlacklistValid(FileInfo file)
        {
            return _settings.FileExtensionBlacklist == null || _settings.FileExtensionBlacklist.Count == 0 ||
                   !_settings.FileExtensionBlacklist.Any(
                       x => x.EndsWith(file.Extension, StringComparison.OrdinalIgnoreCase));
        }

        public void SetSettings(DirectoryWatcherSettings settings)
        {
            _settings = settings;
            UpdateTimerInterval();
        }

        public void SetWatchPaths(IEnumerable<string> paths)
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

        private void UpdateTimerInterval()
        {
            _dirWatcherTimer.Interval = TimeSpan.FromMilliseconds(_settings.CheckInterval);
            if (_dirWatcherTimer.IsEnabled)
            {
                _dirWatcherTimer.Stop();
                _dirWatcherTimer.Start();
            }
        }
    }
}