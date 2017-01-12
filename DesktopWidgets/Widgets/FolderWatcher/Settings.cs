using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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

        [Category("Style")]
        [DisplayName("Show File Date/Time")]
        public bool ShowFileDateTime { get; set; } = false;

        [Category("Style")]
        [DisplayName("File Name Font Settings")]
        public FontSettings FileNameFontSettings { get; set; } = new FontSettings();

        [Category("Behavior")]
        [DisplayName("Change Current File On Detect")]
        public bool QueueFiles { get; set; } = true;

        [Category("Content")]
        [DisplayName("Show Images")]
        public bool ShowImages { get; set; } = true;

        [Category("Content")]
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

        [Category("Content")]
        [DisplayName("Show Text Content Max Size (bytes)")]
        public int ShowContentMaxSize { get; set; } = 1048576;

        [Category("Style")]
        [DisplayName("Text Content Font Settings")]
        public FontSettings ContentFontSettings { get; set; } = new FontSettings();

        [Category("Style")]
        [DisplayName("Show Text Content Horizontal Alignment")]
        public HorizontalAlignment ShowContentHorizontalAlignment { get; set; } = HorizontalAlignment.Stretch;

        [Category("Style")]
        [DisplayName("Show Text Content Vertical Alignment")]
        public VerticalAlignment ShowContentVerticalAlignment { get; set; } = VerticalAlignment.Stretch;

        [Category("Content")]
        [DisplayName("Play Media Volume")]
        public double PlayMediaVolume { get; set; } = 1.0;

        [Category("Content")]
        [DisplayName("Play Media On Detect")]
        public bool PlayMedia { get; set; } = false;

        [DisplayName("Current File")]
        public FileInfo CurrentFile { get; set; }

        [Category("General")]
        [DisplayName("Paused")]
        public bool Paused { get; set; } = false;

        [Category("Behavior")]
        [DisplayName("Resume On Manual Change Duration")]
        public TimeSpan ResumeWaitDuration { get; set; } = TimeSpan.FromSeconds(10);

        [Browsable(false)]
        [DisplayName("History")]
        public List<FileInfo> FileHistory { get; set; } = new List<FileInfo>();

        [Category("General")]
        [DisplayName("Max History")]
        public int FileHistoryMax { get; set; } = 99;

        [Category("General")]
        [DisplayName("Resume On Start Mode")]
        public ResumeOnStartMode ResumeOnStartMode { get; set; } = ResumeOnStartMode.Auto;

        [Browsable(false)]
        [DisplayName("Resume On Start")]
        public bool ResumeOnNextStart { get; set; } = false;
    }
}