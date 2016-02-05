using System;
using System.Collections.Generic;
using System.ComponentModel;
using DesktopWidgets.WidgetBase.Interfaces;
using DesktopWidgets.WidgetBase.Settings;

namespace DesktopWidgets.Widgets.RSSFeed
{
    public class Settings : WidgetSettingsBase, IEventWidget
    {
        [Category("Style")]
        [DisplayName("Max Headlines")]
        public int MaxHeadlines { get; set; }

        [Category("General")]
        [DisplayName("Refresh Interval")]
        public TimeSpan RefreshInterval { get; set; }

        [Category("Feed")]
        [DisplayName("URL")]
        public string RssFeedUrl { get; set; }

        [Category("Feed (Filter)")]
        [DisplayName("Title Whitelist")]
        public List<string> TitleWhitelist { get; set; }

        [Category("Feed (Filter)")]
        [DisplayName("Title Blacklist")]
        public List<string> TitleBlacklist { get; set; }

        [Category("Feed (Filter)")]
        [DisplayName("Category Whitelist")]
        public List<string> CategoryWhitelist { get; set; }

        [Category("Behavior")]
        [DisplayName("New Headline Sound Path")]
        public string EventSoundPath { get; set; }

        [Category("Behavior")]
        [DisplayName("New Headline Sound Volume")]
        public double EventSoundVolume { get; set; }

        [Category("Style")]
        [DisplayName("Show Publish Date")]
        public bool ShowPublishDate { get; set; }

        [Category("Style")]
        [DisplayName("Publish Date Font Size")]
        public int PublishDateFontSize { get; set; }

        [Category("Style")]
        [DisplayName("Publish Date Format")]
        public string PublishDateFormat { get; set; }

        [Category("Style")]
        [DisplayName("Publish Date Time Offset")]
        public TimeSpan PublishDateTimeOffset { get; set; }

        [Category("Behavior (Hideable)")]
        [DisplayName("Show On New Headline")]
        public bool OpenOnEvent { get; set; }

        [Category("Behavior (Hideable)")]
        [DisplayName("Stay Open On New Headline")]
        public bool OpenOnEventStay { get; set; }

        [Category("Behavior (Hideable)")]
        [DisplayName("Show On New Headline Duration")]
        public TimeSpan OpenOnEventDuration { get; set; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            MaxHeadlines = 5;
            RefreshInterval = TimeSpan.FromHours(1);
            RssFeedUrl = "";
            TitleWhitelist = new List<string>();
            TitleBlacklist = new List<string>();
            CategoryWhitelist = new List<string>();
            EventSoundPath = "";
            EventSoundVolume = 1.0;
            ShowPublishDate = false;
            PublishDateFontSize = 11;
            PublishDateFormat = "";
            PublishDateTimeOffset = TimeSpan.FromHours(0);
            OpenOnEvent = true;
            OpenOnEventStay = false;
            OpenOnEventDuration = TimeSpan.FromSeconds(10);
        }
    }
}