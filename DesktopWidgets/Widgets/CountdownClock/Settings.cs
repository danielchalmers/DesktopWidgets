using System;
using System.ComponentModel;
using DesktopWidgets.WidgetBase.Settings;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace DesktopWidgets.Widgets.CountdownClock
{
    public class Settings : WidgetClockSettingsBase
    {
        public Settings()
        {
            Format = "dd'd 'hh'h 'mm'm'";
        }

        [Category("End")]
        [DisplayName("Date/Time")]
        public DateTime EndDateTime { get; set; } = DateTime.Now;

        [Browsable(false)]
        [DisplayName("Last End Date/Time")]
        public DateTime LastEndDateTime { get; set; } = DateTime.Now;

        [Category("Style")]
        [DisplayName("Continue Counting")]
        public bool EndContinueCounting { get; set; } = false;

        [PropertyOrder(0)]
        [Category("End Sync")]
        [DisplayName("Sync Next Year")]
        public bool SyncYear { get; set; } = false;

        [PropertyOrder(1)]
        [Category("End Sync")]
        [DisplayName("Sync Next Month")]
        public bool SyncMonth { get; set; } = false;

        [PropertyOrder(2)]
        [Category("End Sync")]
        [DisplayName("Sync Next Day")]
        public bool SyncDay { get; set; } = false;

        [PropertyOrder(3)]
        [Category("End Sync")]
        [DisplayName("Sync Next Hour")]
        public bool SyncHour { get; set; } = false;

        [PropertyOrder(4)]
        [Category("End Sync")]
        [DisplayName("Sync Next Minute")]
        public bool SyncMinute { get; set; } = false;

        [PropertyOrder(5)]
        [Category("End Sync")]
        [DisplayName("Sync Next Second")]
        public bool SyncSecond { get; set; } = false;
    }
}