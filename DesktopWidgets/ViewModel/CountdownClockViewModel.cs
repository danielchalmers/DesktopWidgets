using System;

namespace DesktopWidgets.ViewModel
{
    public class CountdownClockViewModel : ClockViewModel
    {
        public CountdownClockViewModel(Guid guid) : base(guid)
        {
            Settings = WidgetHelper.GetWidgetSettingsFromGuid(guid) as WidgetCountdownClockSettings;
            if (Settings == null)
                return;
        }

        public WidgetCountdownClockSettings Settings { get; }
    }
}