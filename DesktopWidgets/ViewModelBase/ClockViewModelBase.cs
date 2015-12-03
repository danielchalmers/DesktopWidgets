using System;
using System.Windows.Threading;
using DesktopWidgets.Classes;
using DesktopWidgets.Helpers;

namespace DesktopWidgets.ViewModelBase
{
    public class ClockViewModelBase : WidgetViewModelBase
    {
        private readonly DispatcherTimer _clockUpdateTimer;
        private readonly WidgetClockSettingsBase _settings;
        private DateTime _currentTime;

        public ClockViewModelBase(WidgetId id, bool startTicking = true) : base(id)
        {
            _settings = id.GetSettings() as WidgetClockSettingsBase;
            if (_settings == null)
                return;
            _clockUpdateTimer = new DispatcherTimer();
            _clockUpdateTimer.Tick += (sender, args) => UpdateCurrentTime();
            UpdateCurrentTime();
            if (startTicking)
                StartClockUpdateTimer();
        }

        public DateTime CurrentTime
        {
            get { return _currentTime; }
            set
            {
                if (_currentTime != value)
                {
                    _currentTime = value;
                    RaisePropertyChanged(nameof(CurrentTime));
                }
            }
        }

        private void SyncClockUpdateInterval()
        {
            var newTime = _settings.UpdateInterval > 0
                ? _settings.UpdateInterval
                : (1000 - DateTime.Now.Millisecond);
            _clockUpdateTimer.Interval = TimeSpan.FromMilliseconds(newTime);
            if (_clockUpdateTimer.IsEnabled)
            {
                _clockUpdateTimer.Stop();
                _clockUpdateTimer.Start();
            }
        }

        public void UpdateCurrentTime()
        {
            CurrentTime = DateTime.Now;
            SyncClockUpdateInterval();
        }

        public void StartClockUpdateTimer()
        {
            SyncClockUpdateInterval();
            _clockUpdateTimer.Start();
        }

        public void StopClockUpdateTimer()
        {
            _clockUpdateTimer.Stop();
        }
    }
}