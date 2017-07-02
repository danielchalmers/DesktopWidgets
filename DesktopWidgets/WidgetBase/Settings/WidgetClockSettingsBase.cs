using System;
using System.ComponentModel;

namespace DesktopWidgets.WidgetBase.Settings
{
    public abstract class WidgetClockSettingsBase : WidgetSettingsBase
    {
        protected WidgetClockSettingsBase()
        {
            Style.FontSettings.FontSize = 24;
        }

        [Category("General")]
        [DisplayName("Update Interval")]
        public int UpdateInterval { get; set; } = -1;

        [Category("Style")]
        [DisplayName("Date/Time Format")]
        public string Format { get; set; } = "hh':'mm' 'tt";

        [Category("General")]
        [DisplayName("Date/Time Offset")]
        public TimeSpan TimeOffset { get; set; }

        [Category("Behavior")]
        [DisplayName("Copy Text On Double Click")]
        public bool CopyTextOnDoubleClick { get; set; } = true;
    }
}