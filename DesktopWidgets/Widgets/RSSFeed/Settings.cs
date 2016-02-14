using System;
using System.Collections.Generic;
using System.ComponentModel;
using DesktopWidgets.WidgetBase.Settings;

namespace DesktopWidgets.Widgets.RSSFeed
{
    public class Settings : WidgetSettingsBase
    {
        [Category("Style")]
        [DisplayName("Max Headlines")]
        public int MaxHeadlines { get; set; } = 5;

        [Category("General")]
        [DisplayName("Refresh Interval")]
        public TimeSpan RefreshInterval { get; set; } = TimeSpan.FromHours(1);

        [Category("Feed")]
        [DisplayName("URL")]
        public string RssFeedUrl { get; set; }

        [Category("Feed (Filter)")]
        [DisplayName("Title Whitelist")]
        public List<string> TitleWhitelist { get; set; } = new List<string>();

        [Category("Feed (Filter)")]
        [DisplayName("Title Blacklist")]
        public List<string> TitleBlacklist { get; set; } = new List<string>();

        [Category("Feed (Filter)")]
        [DisplayName("Category Whitelist")]
        public List<string> CategoryWhitelist { get; set; } = new List<string>();

        [Category("Style")]
        [DisplayName("Show Publish Date")]
        public bool ShowPublishDate { get; set; }

        [Category("Style")]
        [DisplayName("Publish Date Font Size")]
        public int PublishDateFontSize { get; set; } = 11;

        [Category("Style")]
        [DisplayName("Publish Date Format")]
        public string PublishDateFormat { get; set; } = "";

        [Category("Style")]
        [DisplayName("Publish Date Time Offset")]
        public TimeSpan PublishDateTimeOffset { get; set; }
    }
}