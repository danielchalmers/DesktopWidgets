using System;
using System.Windows.Threading;

namespace DesktopWidgets.ViewModel
{
    public class ClockViewModel : WidgetViewModelBase
    {
        private readonly DispatcherTimer _clockUpdateTimer;
        private readonly WidgetClockSettings _settings;
        private DateTime _currentTime;

        public ClockViewModel(Guid guid) : base(guid)
        {
            _settings = WidgetHelper.GetWidgetSettingsFromGuid(guid) as WidgetClockSettings;
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