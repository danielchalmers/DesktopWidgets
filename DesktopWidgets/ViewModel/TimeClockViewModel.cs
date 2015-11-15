using System;

namespace DesktopWidgets.ViewModel
{
    public class TimeClockViewModel : ClockViewModel
    {
        public TimeClockViewModel(Guid guid) : base(guid)
        {
            Settings = WidgetHelper.GetWidgetSettingsFromGuid(guid) as WidgetTimeClockSettings;
            if (Settings == null)
                return;
        }

        public WidgetTimeClockSettings Settings { get; }
    }
}