using System;
using System.Collections.Generic;
using System.ComponentModel;
using DesktopWidgets.Classes;
using DesktopWidgets.WidgetBase.Settings;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace DesktopWidgets.Widgets.RSSFeed
{
    public class Settings : WidgetSettingsBase
    {
        public Settings()
        {
            Style.FontSettings.FontSize = 16;
        }

        [Category("Style")]
        [DisplayName("Max Headlines")]
        public int MaxHeadlines { get; set; } = 5;

        [Category("General")]
        [DisplayName("Refresh Interval")]
        public TimeSpan RefreshInterval { get; set; } = TimeSpan.FromHours(1);

        [PropertyOrder(0)]
        [Category("General")]
        [DisplayName("URL")]
        public string RssFeedUrl { get; set; }

        [PropertyOrder(1)]
        [Category("General")]
        [DisplayName("Title Whitelist")]
        public List<string> TitleWhitelist { get; set; } = new List<string>();

        [PropertyOrder(2)]
        [Category("General")]
        [DisplayName("Title Blacklist")]
        public List<string> TitleBlacklist { get; set; } = new List<string>();

        [PropertyOrder(3)]
        [Category("General")]
        [DisplayName("Category Whitelist")]
        public List<string> CategoryWhitelist { get; set; } = new List<string>();

        [Category("Style")]
        [DisplayName("Show Publish Date")]
        public bool ShowPublishDate { get; set; }

        [Category("Style")]
        [DisplayName("Publish Date Font Settings")]
        public FontSettings PublishDateFontSettings { get; set; } = new FontSettings { FontSize = 11 };

        [Category("Style")]
        [DisplayName("Publish Date Format")]
        public string PublishDateFormat { get; set; } = "";

        [Category("Style")]
        [DisplayName("Publish Date Time Offset")]
        public TimeSpan PublishDateTimeOffset { get; set; }
    }
}