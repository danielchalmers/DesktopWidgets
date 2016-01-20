using System;
using DesktopWidgets.Classes;
using DesktopWidgets.Helpers;
using DesktopWidgets.ViewModelBase;

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
                Settings.EndDateTime = new DateTime(
                    Settings.SyncYear && Settings.EndDateTime.Year != DateTime.Now.Year
                        ? DateTime.Now.Year
                        : Settings.EndDateTime.Year,
                    Settings.SyncMonth && Settings.EndDateTime.Month != DateTime.Now.Month
                        ? DateTime.Now.Month
                        : Settings.EndDateTime.Month,
                    Settings.SyncDay && Settings.EndDateTime.Day != DateTime.Now.Day
                        ? DateTime.Now.Day
                        : Settings.EndDateTime.Day,
                    Settings.SyncHour && Settings.EndDateTime.Hour != DateTime.Now.Hour
                        ? DateTime.Now.Hour
                        : Settings.EndDateTime.Hour,
                    Settings.SyncMinute && Settings.EndDateTime.Minute != DateTime.Now.Minute
                        ? DateTime.Now.Minute
                        : Settings.EndDateTime.Minute,
                    Settings.SyncSecond && Settings.EndDateTime.Second != DateTime.Now.Second
                        ? DateTime.Now.Second
                        : Settings.EndDateTime.Second,
                    Settings.EndDateTime.Kind);
        }

        private void OnTickAction()
        {
            SyncTime();

            if (CurrentTime >= Settings.EndDateTime && Settings.LastEndDateTime != Settings.EndDateTime)
            {
                Settings.LastEndDateTime = Settings.EndDateTime;
                OnEndAction();
            }
        }

        private void OnEndAction()
        {
            if (!App.IsMuted)
                MediaPlayerStore.PlaySoundAsync(Settings.EndSoundPath, Settings.EndSoundVolume);
            if (Settings.OpenOnEvent)
                Settings.Identifier.GetView()
                    .ShowIntro(Settings.OpenOnEventStay ? 0 : (int) Settings.OpenOnEventDuration.TotalMilliseconds);
        }
    }
}