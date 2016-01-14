using System;
using System.ComponentModel;
using DesktopWidgets.Classes;

namespace DesktopWidgets.Widgets.FolderWatcher
{
    public class Settings : WidgetSettingsBase, IEventWidget
    {
        public Settings()
        {
            Width = 384;
            Height = 216;
        }

        [Category("General")]
        [DisplayName("Folder Check Interval")]
        public TimeSpan FolderCheckInterval { get; set; } = TimeSpan.FromMilliseconds(500);

        [Category("General")]
        [DisplayName("Watch Folder")]
        public string WatchFolder { get; set; } = "";

        [Category("General")]
        [DisplayName("Include Filter")]
        public string IncludeFilter { get; set; } = "*.*";

        [Category("General")]
        [DisplayName("Exclude Filter")]
        public string ExcludeFilter { get; set; } = "";

        [Category("Style")]
        [DisplayName("Show File Name")]
        public bool ShowFileName { get; set; } = true;

        [Category("Style")]
        [DisplayName("Show Mute")]
        public bool ShowMute { get; set; } = true;

        [Category("Style")]
        [DisplayName("Show Dismiss")]
        public bool ShowDismiss { get; set; } = true;

        [DisplayName("Mute End Time")]
        public DateTime MuteEndTime { get; set; } = DateTime.Now;

        [DisplayName("Mute Duration")]
        public TimeSpan MuteDuration { get; set; } = TimeSpan.FromHours(1);

        [Category("General")]
        [DisplayName("Timeout Duration")]
        public TimeSpan TimeoutDuration { get; set; } = TimeSpan.FromMinutes(1);

        [DisplayName("Last File Check")]
        public DateTime LastCheck { get; set; } = DateTime.Now;

        [Category("General")]
        [DisplayName("Enable Timeout")]
        public bool EnableTimeout { get; set; } = false;

        [Category("Behavior")]
        [DisplayName("Event Sound Path")]
        public string EventSoundPath { get; set; }

        [Category("Behavior")]
        [DisplayName("Event Sound Volume")]
        public double EventSoundVolume { get; set; } = 1.0;

        [Category("Behavior")]
        [DisplayName("Replace Existing File")]
        public bool ReplaceExistingFile { get; set; } = false;

        [Category("Behavior (Hideable)")]
        [DisplayName("Open On Event")]
        public bool OpenOnEvent { get; set; } = true;

        [Category("Behavior (Hideable)")]
        [DisplayName("Stay Open On Event")]
        public bool OpenOnEventStay { get; set; } = false;

        [Category("Behavior (Hideable)")]
        [DisplayName("Open On Event Duration")]
        public TimeSpan OpenOnEventDuration { get; set; } = TimeSpan.FromSeconds(10);
    }
}