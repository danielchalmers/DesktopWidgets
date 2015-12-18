using System;
using System.ComponentModel;
using DesktopWidgets.Classes;

namespace DesktopWidgets.Widgets.CountdownClock
{
    public class Settings : WidgetClockSettingsBase
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
    }
}