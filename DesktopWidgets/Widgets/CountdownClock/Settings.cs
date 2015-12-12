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

        [Category("General")]
        [DisplayName("End Date/Time")]
        public DateTime EndDateTime { get; set; } = DateTime.Now;

        [Category("General")]
        [DisplayName("Last End Date/Time")]
        public DateTime LastEndDateTime { get; set; } = DateTime.Now;

        [Category("General")]
        [DisplayName("End Sound Path")]
        public string EndSoundPath { get; set; }

        [Category("General")]
        [DisplayName("End Sound Volume")]
        public double EndSoundVolume { get; set; } = 1;

        [Category("General")]
        [DisplayName("Continue Counting")]
        public bool EndContinueCounting { get; set; } = false;
    }
}