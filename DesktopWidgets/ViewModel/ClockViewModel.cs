using System;
using System.Windows.Threading;

namespace DesktopWidgets.ViewModel
{
    public class ClockViewModel : WidgetViewModelBase
    {
        private readonly WidgetClockSettings _settings;
        private DateTime _currentTime;

        public ClockViewModel(Guid guid) : base(guid)
        {
            _settings = WidgetHelper.GetWidgetSettingsFromGuid(guid) as WidgetClockSettings;
            if (_settings == null)
                return;
            var clockTimer = new DispatcherTimer {Interval = _settings.TickInterval};
            clockTimer.Tick += delegate { CurrentTime = DateTime.Now; };
            clockTimer.Start();
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
    }
}