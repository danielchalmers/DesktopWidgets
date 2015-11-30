using System;
using DesktopWidgets.Helpers;
using DesktopWidgets.ViewModelBase;

namespace DesktopWidgets.Widgets.TimeClock
{
    public class ViewModel : ClockViewModelBase
    {
        public ViewModel(Guid guid) : base(guid)
        {
            Settings = WidgetHelper.GetWidgetSettingsFromGuid(guid) as Settings;
            if (Settings == null)
                return;
        }

        public Settings Settings { get; }
    }
}