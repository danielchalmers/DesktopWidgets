using System;
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
            {
                return;
            }
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
            {
                Settings.EndDateTime = Settings.EndDateTime.SyncNext(
                    CurrentTime,
                    Settings.SyncYear,
                    Settings.SyncMonth,
                    Settings.SyncDay,
                    Settings.SyncHour,
                    Settings.SyncMinute,
                    Settings.SyncSecond);
            }
        }

        protected override void UpdateTimer_Tick(object sender, EventArgs e)
        {
            base.UpdateTimer_Tick(sender, e);
            if (CurrentTime >= Settings.EndDateTime && Settings.LastEndDateTime != Settings.EndDateTime)
            {
                Settings.LastEndDateTime = Settings.EndDateTime;
                OnSpecialEvent();
            }

            SyncTime();
        }
    }
}