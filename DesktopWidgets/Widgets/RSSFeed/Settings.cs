using System;
using System.ComponentModel;
using DesktopWidgets.Classes;

namespace DesktopWidgets.Widgets.RSSFeed
{
    public class Settings : WidgetSettingsBase, IEventWidget
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

        [Category("Feed")]
        [DisplayName("Title Whitelist")]
        public string RssFeedTitleWhitelist { get; set; }

        [Category("Feed")]
        [DisplayName("Title Blacklist")]
        public string RssFeedTitleBlacklist { get; set; }

        [Category("Behavior")]
        [DisplayName("New Headline Sound Path")]
        public string EventSoundPath { get; set; }

        [Category("Behavior")]
        [DisplayName("New Headline Sound Volume")]
        public double EventSoundVolume { get; set; } = 1.0;

        [Category("Behavior (Hideable)")]
        [DisplayName("Open On New Headline")]
        public bool OpenOnEvent { get; set; } = true;

        [Category("Behavior (Hideable)")]
        [DisplayName("Stay Open On New Headline")]
        public bool OpenOnEventStay { get; set; } = false;

        [Category("Behavior (Hideable)")]
        [DisplayName("Open On New Headline Duration")]
        public TimeSpan OpenOnEventDuration { get; set; } = TimeSpan.FromSeconds(10);
    }
}