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
        [PropertyOrder(0)]
        [DisplayName("Watch Folder Paths")]
        public List<string> WatchFolders { get; set; } = new List<string>();

        [PropertyOrder(4)]
        [DisplayName("File Extension Whitelist")]
        public List<string> FileExtensionWhitelist { get; set; } = new List<string>();

        [PropertyOrder(5)]
        [DisplayName("File Extension Blacklist")]
        public List<string> FileExtensionBlacklist { get; set; } = new List<string>();

        [PropertyOrder(1)]
        [DisplayName("Recursive")]
        public bool Recursive { get; set; } = false;

        [PropertyOrder(7)]
        [DisplayName("Check Interval (ms)")]
        public int CheckInterval { get; set; } = 500;

        [PropertyOrder(6)]
        [DisplayName("Max File Size (bytes)")]
        public double MaxSize { get; set; } = 0;

        [PropertyOrder(2)]
        [DisplayName("Detect New Files")]
        public bool DetectNewFiles { get; set; } = true;

        [PropertyOrder(3)]
        [DisplayName("Detect Modified Files")]
        public bool DetectModifiedFiles { get; set; } = true;

        [PropertyOrder(8)]
        [DisplayName("Timeout Duration")]
        public TimeSpan TimeoutDuration { get; set; } = TimeSpan.FromMinutes(0);

        [Browsable(false)]
        [DisplayName("Last Check")]
        public DateTime LastCheck { get; set; } = DateTime.Now;

        public override string ToString()
        {
            return WatchFolders.Count > 0 ? string.Join(", ", WatchFolders) : "None";
        }
    }
}