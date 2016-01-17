using System;
using System.ComponentModel;
using DesktopWidgets.Classes;

namespace DesktopWidgets.Widgets.CountdownClock
{
    public class Settings : WidgetClockSettingsBase, IEventWidget
    {
        public Settings()
        {
            TimeFormat = "dd:hh:mm:ss";
        }

        [Category("End")]
        [DisplayName("Date/Time")]
        public DateTime EndDateTime { get; set; } = DateTime.Now;

        [DisplayName("Last End Date/Time")]
        public DateTime LastEndDateTime { get; set; } = DateTime.Now;

        [Category("End")]
        [DisplayName("Sound Path")]
        public string EndSoundPath { get; set; }

        [Category("End")]
        [DisplayName("Sound Volume")]
        public double EndSoundVolume { get; set; } = 1;

        [Category("Style")]
        [DisplayName("Continue Counting")]
        public bool EndContinueCounting { get; set; } = false;

        [Category("End Sync")]
        [DisplayName("Sync Year")]
        public bool SyncYear { get; set; } = false;

        [Category("End Sync")]
        [DisplayName("Sync Month")]
        public bool SyncMonth { get; set; } = false;

        [Category("End Sync")]
        [DisplayName("Sync Day")]
        public bool SyncDay { get; set; } = false;

        [Category("End Sync")]
        [DisplayName("Sync Hour")]
        public bool SyncHour { get; set; } = false;

        [Category("End Sync")]
        [DisplayName("Sync Minute")]
        public bool SyncMinute { get; set; } = false;

        [Category("End Sync")]
        [DisplayName("Sync Second")]
        public bool SyncSecond { get; set; } = false;

        [Category("Behavior (Hideable)")]
        [DisplayName("Open On Event")]
        public bool OpenOnEvent { get; set; } = true;

        [Category("Behavior (Hideable)")]
        [DisplayName("Stay Open On Event")]
        public bool OpenOnEventStay { get; set; } = false;

        [Category("Behavior (Hideable)")]
        [DisplayName("Open On Event Duration")]
        public TimeSpan OpenOnEventDuration { get; set; } = TimeSpan.FromSeconds(10);
    }
}