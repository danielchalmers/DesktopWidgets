using DesktopWidgets.Helpers;
using DesktopWidgets.WidgetBase;
using DesktopWidgets.WidgetBase.ViewModel;

namespace DesktopWidgets.Widgets.CountdownClock
{
    public class ViewModel : ClockViewModelBase
    {
        public ViewModel(WidgetId guid) : base(guid)
        {
            Settings = guid.GetSettings() as Settings;
            if (Settings == null)
                return;
            TickAction = OnTickAction;
        }

        public Settings Settings { get; }

        private void SyncTime()
        {
            if (Settings.SyncYear ||
                Settings.SyncMonth ||
                Settings.SyncDay ||
                Settings.SyncHour ||
                Settings.SyncMinute ||
                Settings.SyncSecond)
                Settings.EndDateTime = Settings.EndDateTime.SyncNext(
                    Settings.SyncYear,
                    Settings.SyncMonth,
                    Settings.SyncDay,
                    Settings.SyncHour,
                    Settings.SyncMinute,
                    Settings.SyncSecond);
        }

        private void OnTickAction()
        {
            if (CurrentTime >= Settings.EndDateTime && Settings.LastEndDateTime != Settings.EndDateTime)
            {
                Settings.LastEndDateTime = Settings.EndDateTime;
                OnSpecialEvent();
            }

            SyncTime();
        }
    }
}