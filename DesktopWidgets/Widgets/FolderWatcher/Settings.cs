using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using DesktopWidgets.Classes;
using DesktopWidgets.WidgetBase.Settings;

namespace DesktopWidgets.Widgets.FolderWatcher
{
    public class Settings : WidgetSettingsBase
    {
        public Settings()
        {
            Style.Width = 384;
            Style.Height = 216;
        }

        [Category("General")]
        public DirectoryWatcherSettings DirectoryWatcherSettings { get; set; } = new DirectoryWatcherSettings();

        [Category("Style")]
        [DisplayName("Show File Name")]
        public bool ShowFileName { get; set; } = true;

        [Category("Behavior")]
        [DisplayName("Enable File Queue")]
        public bool QueueFiles { get; set; } = true;

        [Category("Style")]
        [DisplayName("Show Images")]
        public bool ShowImages { get; set; } = true;

        [Category("General")]
        [DisplayName("Show Text Content Whitelist")]
        public List<string> ShowTextContentWhitelist { get; set; } = new List<string>
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

        [Category("General")]
        [DisplayName("Show Text Content Max Size (bytes)")]
        public int ShowContentMaxSize { get; set; } = 1048576;

        [Category("Style")]
        [DisplayName("Show Text Content Font Size")]
        public int ShowContentFontSize { get; set; } = 12;

        [Category("Style")]
        [DisplayName("Show Text Content Horizontal Alignment")]
        public HorizontalAlignment ShowContentHorizontalAlignment { get; set; } = HorizontalAlignment.Stretch;

        [Category("Style")]
        [DisplayName("Show Text Content Vertical Alignment")]
        public VerticalAlignment ShowContentVerticalAlignment { get; set; } = VerticalAlignment.Stretch;

        [Category("General")]
        [DisplayName("Play Media Volume")]
        public double PlayMediaVolume { get; set; } = 1.0;

        [Category("General")]
        [DisplayName("Play Media On Detect")]
        public bool PlayMedia { get; set; } = false;

        [DisplayName("Current File")]
        public string CurrentFilePath { get; set; }

        [Category("General")]
        [DisplayName("Paused")]
        public bool Paused { get; set; } = false;

        [Category("Behavior")]
        [DisplayName("Resume On Manual Change Duration")]
        public TimeSpan ResumeWaitDuration { get; set; } = TimeSpan.FromSeconds(10);
    }
}