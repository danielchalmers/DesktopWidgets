using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using DesktopWidgets.WidgetBase.Interfaces;
using DesktopWidgets.WidgetBase.Settings;

namespace DesktopWidgets.Widgets.FolderWatcher
{
    public class Settings : WidgetSettingsBase, IEventWidget
    {
        [Category("General")]
        [DisplayName("Folder Check Interval (ms)")]
        public int FolderCheckIntervalMS { get; set; }

        [Category("General")]
        [DisplayName("Watch Folder Path")]
        public string WatchFolder { get; set; }

        [Category("General")]
        [DisplayName("File Extension Whitelist")]
        public List<string> FileExtensionWhitelist { get; set; }

        [Category("General")]
        [DisplayName("File Extension Blacklist")]
        public List<string> FileExtensionBlacklist { get; set; }

        [Category("Style")]
        [DisplayName("Show File Name")]
        public bool ShowFileName { get; set; }

        [Category("General")]
        [DisplayName("Timeout Duration")]
        public TimeSpan TimeoutDuration { get; set; }

        [DisplayName("Last File Check")]
        public DateTime LastCheck { get; set; }

        [Category("General")]
        [DisplayName("Enable Timeout")]
        public bool EnableTimeout { get; set; }

        [Category("Behavior")]
        [DisplayName("Event Sound Path")]
        public string EventSoundPath { get; set; }

        [Category("Behavior")]
        [DisplayName("Event Sound Volume")]
        public double EventSoundVolume { get; set; }

        [Category("Behavior")]
        [DisplayName("Enable File Queue")]
        public bool QueueFiles { get; set; }

        [Category("General")]
        [DisplayName("Recursive")]
        public bool Recursive { get; set; }

        [Category("Behavior")]
        [DisplayName("Detect Modified Files")]
        public bool DetectModifiedFiles { get; set; }

        [Category("Behavior")]
        [DisplayName("Detect New Files")]
        public bool DetectNewFiles { get; set; }

        [Category("Style")]
        [DisplayName("Show Images")]
        public bool ShowImages { get; set; }

        [Category("General")]
        [DisplayName("Show Text Content Whitelist")]
        public List<string> ShowTextContentWhitelist { get; set; }

        [Category("Style")]
        [DisplayName("Show Text Content Font Size")]
        public int ShowContentFontSize { get; set; }

        [Category("Style")]
        [DisplayName("Show Text Content Horizontal Alignment")]
        public HorizontalAlignment ShowContentHorizontalAlignment { get; set; }

        [Category("Style")]
        [DisplayName("Show Text Content Vertical Alignment")]
        public VerticalAlignment ShowContentVerticalAlignment { get; set; }

        [Category("General")]
        [DisplayName("Play Media Volume")]
        public double PlayMediaVolume { get; set; }

        [Category("General")]
        [DisplayName("Play Media On Detect")]
        public bool PlayMedia { get; set; }

        [DisplayName("Current File")]
        public string CurrentFilePath { get; set; }

        [Category("Behavior (Hideable)")]
        [DisplayName("Show On Event")]
        public bool OpenOnEvent { get; set; }

        [Category("Behavior (Hideable)")]
        [DisplayName("Stay Open On Event")]
        public bool OpenOnEventStay { get; set; }

        [Category("Behavior (Hideable)")]
        [DisplayName("Show On Event Duration")]
        public TimeSpan OpenOnEventDuration { get; set; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            FolderCheckIntervalMS = 500;
            WatchFolder = "";
            FileExtensionWhitelist = new List<string>();
            FileExtensionBlacklist = new List<string>();
            ShowFileName = true;
            TimeoutDuration = TimeSpan.FromMinutes(1);
            LastCheck = DateTime.Now;
            EnableTimeout = false;
            EventSoundPath = "";
            EventSoundVolume = 1.0;
            QueueFiles = true;
            Recursive = false;
            DetectModifiedFiles = true;
            DetectNewFiles = true;
            ShowImages = true;
            ShowTextContentWhitelist = new List<string>
            {
                ".txt",
                ".cfg",
                ".json",
                ".xml",
                ".log",
                ".ini",
                ".inf",
                ".properties"
            };
            ShowContentFontSize = 12;
            ShowContentHorizontalAlignment = HorizontalAlignment.Stretch;
            ShowContentVerticalAlignment = VerticalAlignment.Stretch;
            PlayMediaVolume = 1.0;
            PlayMedia = false;
            CurrentFilePath = "";
            OpenOnEvent = true;
            OpenOnEventStay = false;
            OpenOnEventDuration = TimeSpan.FromSeconds(10);
            Width = 384;
            Height = 216;
        }
    }
}