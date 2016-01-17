using System;

namespace DesktopWidgets.Classes
{
    public class DirectoryWatcherSettings
    {
        public string WatchFolder { get; set; }
        public string ExcludeFilter { get; set; }
        public string IncludeFilter { get; set; }
        public bool Recursive { get; set; }
        public TimeSpan CheckInterval { get; set; }
        public double MaxSize { get; set; }
    }
}