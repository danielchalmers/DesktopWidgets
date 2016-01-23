﻿using System;
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

        public DirectoryWatcher(DirectoryWatcherSettings settings,
            Action<FileInfo, DirectoryChange> newFileAction = null)
        {
            _settings = settings;
            _newFileAction = newFileAction;
            KnownFilePaths = new Dictionary<string, List<FileInfo>>();
            _dirWatcherTimer = new DispatcherTimer {Interval = _settings.CheckInterval};
            _dirWatcherTimer.Tick += (sender, args) => CheckDirectoryForNewFilesAsync();
        }

        public void CheckDirectoryForNewFilesAsync(bool promptAction = true)
            => new Task(() => { CheckDirectoryForNewFiles(promptAction); }).Start();

        public void CheckDirectoryForNewFiles(bool promptAction = true)
        {
            try
            {
                var folder = _settings.WatchFolder;
                if (string.IsNullOrWhiteSpace(folder) || !Directory.Exists(folder))
                    return;
                if (!KnownFilePaths.ContainsKey(folder))
                    KnownFilePaths.Add(folder, null);
                var files = Directory.EnumerateFiles(folder, "*.*",
                    _settings.Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).AsParallel()
                    .Select(x => new FileInfo(x))
                    .ToList();
                var includeFilter = !string.IsNullOrWhiteSpace(_settings.IncludeFilter)
                    ? _settings.IncludeFilter.Split('|')
                    : null;
                var excludeFilter = !string.IsNullOrWhiteSpace(_settings.ExcludeFilter)
                    ? _settings.ExcludeFilter.Split('|')
                    : null;
                foreach (var file in files)
                {
                    if (KnownFilePaths[folder] == null)
                        break;
                    if (file.Length > _settings.MaxSize && _settings.MaxSize > 0)
                        continue;
                    if (_settings.IncludeFilter != "*.*" && includeFilter != null &&
                        !includeFilter.Any(x => x.EndsWith(file.Extension, StringComparison.OrdinalIgnoreCase)))
                        continue;
                    if (excludeFilter != null &&
                        excludeFilter.Any(x => x.EndsWith(file.Extension, StringComparison.OrdinalIgnoreCase)))
                        continue;
                    if (promptAction)
                    {
                        var sameFiles = KnownFilePaths[folder].Where(x => x.FullName == file.FullName).ToList();
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

        public void SetWatchPath(string path)
        {
            _settings.WatchFolder = path;
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