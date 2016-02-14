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
        private readonly Action<List<FileInfo>, DirectoryChange> _newFileAction;
        public readonly Dictionary<string, List<FileInfo>> KnownFilePaths;
        private bool _isScanning;
        private DirectoryWatcherSettings _settings;

        public DirectoryWatcher(DirectoryWatcherSettings settings,
            Action<List<FileInfo>, DirectoryChange> newFileAction = null)
        {
            _settings = settings;
            _newFileAction = newFileAction;
            KnownFilePaths = new Dictionary<string, List<FileInfo>>();
            _dirWatcherTimer = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(_settings.CheckInterval)};
            _dirWatcherTimer.Tick += (sender, args) => CheckDirectoriesForNewFilesAsync();
        }

        public void CheckDirectoriesForNewFilesAsync()
            => new Task(CheckDirectoriesForNewFiles).Start();

        public void CheckDirectoriesForNewFiles()
        {
            if (_isScanning)
                return;
            _isScanning = true;
            foreach (
                var folder in
                    _settings.WatchFolders.Where(
                        folder => !string.IsNullOrWhiteSpace(folder) && Directory.Exists(folder)))
            {
                try
                {
                    var dirInfo = new DirectoryInfo(folder);
                    if (!KnownFilePaths.ContainsKey(folder))
                        KnownFilePaths.Add(folder, null);
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
                                            y => y.FullName == x.FullName && y.LastWriteTimeUtc != x.LastWriteTimeUtc))
                                    .ToList();
                            Application.Current.Dispatcher.Invoke(
                                () =>
                                {
                                    if (changedFiles.Count > 0)
                                        _newFileAction?.Invoke(changedFiles, DirectoryChange.FileChanged);
                                });
                        }
                        if (_settings.DetectNewFiles)
                        {
                            var newFiles = files.Where(x => oldFiles.All(y => y.FullName != x.FullName)).ToList();
                            Application.Current.Dispatcher.Invoke(
                                () =>
                                {
                                    if (newFiles.Count > 0)
                                        _newFileAction?.Invoke(newFiles, DirectoryChange.NewFile);
                                });
                        }
                    }
                }
                catch
                {
                    // ignored
                }
            }
            _isScanning = false;
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
    }
}