using System;
using System.Collections.Generic;
using System.ComponentModel;
using DesktopWidgets.WidgetBase.Interfaces;
using DesktopWidgets.WidgetBase.Settings;

namespace DesktopWidgets.Widgets.CountdownClock
{
    public class Settings : WidgetClockSettingsBase, IEventWidget
    {
        [Category("End")]
        [DisplayName("Date/Time")]
        public DateTime EndDateTime { get; set; }

        [DisplayName("Last End Date/Time")]
        public DateTime LastEndDateTime { get; set; }

        [Category("End")]
        [DisplayName("Sound Path")]
        public string EndSoundPath { get; set; }

        [Category("End")]
        [DisplayName("Sound Volume")]
        public double EndSoundVolume { get; set; }

        [Category("Style")]
        [DisplayName("Continue Counting")]
        public bool EndContinueCounting { get; set; }

        [Category("End Sync")]
        [DisplayName("Sync Next Year")]
        public bool SyncYear { get; set; }

        [Category("End Sync")]
        [DisplayName("Sync Next Month")]
        public bool SyncMonth { get; set; }

        [Category("End Sync")]
        [DisplayName("Sync Next Day")]
        public bool SyncDay { get; set; }

        [Category("End Sync")]
        [DisplayName("Sync Next Hour")]
        public bool SyncHour { get; set; }

        [Category("End Sync")]
        [DisplayName("Sync Next Minute")]
        public bool SyncMinute { get; set; }

        [Category("End Sync")]
        [DisplayName("Sync Next Second")]
        public bool SyncSecond { get; set; }

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
            EndDateTime = DateTime.Now;
            LastEndDateTime = DateTime.Now;
            EndSoundPath = "";
            EndSoundVolume = 1.0;
            EndContinueCounting = false;
            SyncYear = false;
            SyncMonth = false;
            SyncDay = false;
            SyncHour = false;
            SyncMinute = false;
            SyncSecond = false;
            OpenOnEvent = true;
            OpenOnEventStay = false;
            OpenOnEventDuration = TimeSpan.FromSeconds(10);

            DateTimeFormat = new List<string> {"{dd}d {hh}h {mm}m"};
        }
    }
}