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

        private void OnTickAction()
        {
            if (Settings.SyncYear && Settings.EndDateTime.Year != DateTime.Now.Year)
                Settings.EndDateTime = new DateTime(DateTime.Now.Year, Settings.EndDateTime.Month,
                    Settings.EndDateTime.Day, Settings.EndDateTime.Hour, Settings.EndDateTime.Minute,
                    Settings.EndDateTime.Second, Settings.EndDateTime.Kind);
            if (CurrentTime >= Settings.EndDateTime && Settings.LastEndDateTime != Settings.EndDateTime)
            {
                Settings.LastEndDateTime = Settings.EndDateTime;
                OnEndAction();
            }
        }

        private void OnEndAction()
        {
            MediaPlayerStore.PlaySoundAsync(Settings.EndSoundPath, Settings.EndSoundVolume);
            if (Settings.OpenOnEvent)
                Settings.Identifier.GetView()
                    .ShowIntro(Settings.OpenOnEventStay ? 0 : (int) Settings.OpenOnEventDuration.TotalMilliseconds,
                        false);
        }
    }
}