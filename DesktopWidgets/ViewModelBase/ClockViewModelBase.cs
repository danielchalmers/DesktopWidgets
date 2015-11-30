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

        public ClockViewModelBase(WidgetId id) : base(id)
        {
            _settings = WidgetHelper.GetWidgetSettingsFromId(id) as WidgetClockSettingsBase;
            if (_settings == null)
                return;
            _clockUpdateTimer = new DispatcherTimer {Interval = _settings.TickInterval};
            _clockUpdateTimer.Tick += delegate { CurrentTime = DateTime.Now; };
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

        public void StartClockUpdateTimer()
        {
            _clockUpdateTimer.Start();
        }

        public void StopClockUpdateTimer()
        {
            _clockUpdateTimer.Stop();
        }
    }
}