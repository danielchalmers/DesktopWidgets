using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace DesktopWidgets.Classes
{
    [ExpandableObject]
    [DisplayName("Directory Watcher Settings")]
    public class DirectoryWatcherSettings
    {
        [DisplayName("Watch Folder Paths")]
        public List<string> WatchFolders { get; set; } = new List<string>();

        [DisplayName("File Extension Whitelist")]
        public List<string> FileExtensionWhitelist { get; set; } = new List<string>();

        [DisplayName("File Extension Blacklist")]
        public List<string> FileExtensionBlacklist { get; set; } = new List<string>();

        [DisplayName("Recursive")]
        public bool Recursive { get; set; } = false;

        [DisplayName("Check Interval (ms)")]
        public int CheckInterval { get; set; } = 500;

        [DisplayName("Max File Size (bytes)")]
        public double MaxSize { get; set; } = 0;

        [DisplayName("Detect New Files")]
        public bool DetectNewFiles { get; set; } = true;

        [DisplayName("Detect Modified Files")]
        public bool DetectModifiedFiles { get; set; } = true;

        [DisplayName("Timeout Duration")]
        public TimeSpan TimeoutDuration { get; set; } = TimeSpan.FromMinutes(0);

        [Browsable(false)]
        [DisplayName("Last Check")]
        public DateTime LastCheck { get; set; } = DateTime.Now;
    }
}