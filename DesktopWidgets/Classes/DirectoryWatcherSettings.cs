using System;
using System.Collections.Generic;

namespace DesktopWidgets.Classes
{
    public class DirectoryWatcherSettings
    {
        public List<string> WatchFolders { get; set; }
        public List<string> FileExtensionWhitelist { get; set; }
        public List<string> FileExtensionBlacklist { get; set; }
        public bool Recursive { get; set; }
        public TimeSpan CheckInterval { get; set; }
        public double MaxSize { get; set; }
    }
}