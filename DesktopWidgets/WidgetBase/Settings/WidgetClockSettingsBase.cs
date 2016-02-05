using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DesktopWidgets.WidgetBase.Settings
{
    public class WidgetClockSettingsBase : WidgetSettingsBase
    {
        [Category("General")]
        [DisplayName("Refresh Interval")]
        public int UpdateInterval { get; set; }

        [Category("Style")]
        [DisplayName("Time Format")]
        public List<string> DateTimeFormat { get; set; }

        [Category("General")]
        [DisplayName("Time Offset")]
        public TimeSpan TimeOffset { get; set; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            UpdateInterval = -1;
            DateTimeFormat = new List<string> {"{hh}:{mm} {tt}"};
            TimeOffset = TimeSpan.FromHours(0);
        }
    }
}