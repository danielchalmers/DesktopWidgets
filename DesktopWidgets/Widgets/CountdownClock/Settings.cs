using System;
using System.Collections.Generic;
using System.ComponentModel;
using DesktopWidgets.WidgetBase.Settings;

namespace DesktopWidgets.Widgets.CountdownClock
{
    public class Settings : WidgetClockSettingsBase
    {
        public Settings()
        {
            DateTimeFormat = new List<string> {"{dd}d {hh}h {mm}m"};
        }

        [Category("End")]
        [DisplayName("Date/Time")]
        public DateTime EndDateTime { get; set; } = DateTime.Now;

        [DisplayName("Last End Date/Time")]
        public DateTime LastEndDateTime { get; set; } = DateTime.Now;

        [Category("Style")]
        [DisplayName("Continue Counting")]
        public bool EndContinueCounting { get; set; } = false;

        [Category("End Sync")]
        [DisplayName("Sync Next Year")]
        public bool SyncYear { get; set; } = false;

        [Category("End Sync")]
        [DisplayName("Sync Next Month")]
        public bool SyncMonth { get; set; } = false;

        [Category("End Sync")]
        [DisplayName("Sync Next Day")]
        public bool SyncDay { get; set; } = false;

        [Category("End Sync")]
        [DisplayName("Sync Next Hour")]
        public bool SyncHour { get; set; } = false;

        [Category("End Sync")]
        [DisplayName("Sync Next Minute")]
        public bool SyncMinute { get; set; } = false;

        [Category("End Sync")]
        [DisplayName("Sync Next Second")]
        public bool SyncSecond { get; set; } = false;
    }
}