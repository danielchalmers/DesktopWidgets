using System;
using System.ComponentModel;
using System.Windows;
using DesktopWidgets.WidgetBase.Interfaces;
using DesktopWidgets.WidgetBase.Settings;

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
        [DisplayName("Folder Check Interval (ms)")]
        public int FolderCheckIntervalMS { get; set; } = 500;

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
        [DisplayName("Enable File Queue")]
        public bool QueueFiles { get; set; } = true;

        [Category("General")]
        [DisplayName("Recursive")]
        public bool Recursive { get; set; } = false;

        [Category("Behavior")]
        [DisplayName("Detect Modified Files")]
        public bool DetectModifiedFiles { get; set; } = false;

        [Category("Style")]
        [DisplayName("Show Images")]
        public bool ShowImages { get; set; } = true;

        [Category("General")]
        [DisplayName("Show Content Filter")]
        public string ShowContentFilter { get; set; } = ".txt|.cfg|.json|.xml|.log|.ini|.inf|.properties";

        [Category("Style")]
        [DisplayName("Show Content Font Size")]
        public int ShowContentFontSize { get; set; } = 12;

        [Category("Style")]
        [DisplayName("Show Content Horizontal Alignment")]
        public HorizontalAlignment ShowContentHorizontalAlignment { get; set; } = HorizontalAlignment.Stretch;

        [Category("Style")]
        [DisplayName("Show Content Vertical Alignment")]
        public VerticalAlignment ShowContentVerticalAlignment { get; set; } = VerticalAlignment.Stretch;

        [Category("General")]
        [DisplayName("Play Media Volume")]
        public double PlayMediaVolume { get; set; } = 1.0;

        [Category("General")]
        [DisplayName("Play Media On Detect")]
        public bool PlayMedia { get; set; } = false;

        [Category("Behavior (Hideable)")]
        [DisplayName("Show On Event")]
        public bool OpenOnEvent { get; set; } = true;

        [Category("Behavior (Hideable)")]
        [DisplayName("Stay Open On Event")]
        public bool OpenOnEventStay { get; set; } = false;

        [Category("Behavior (Hideable)")]
        [DisplayName("Show On Event Duration")]
        public TimeSpan OpenOnEventDuration { get; set; } = TimeSpan.FromSeconds(10);
    }
}