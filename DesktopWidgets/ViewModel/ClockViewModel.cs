using System;
using System.Windows.Threading;

namespace DesktopWidgets.ViewModel
{
    public class ClockViewModel : WidgetViewModelBase
    {
        private DateTime _currentTime;

        public ClockViewModel(Guid guid) : base(guid)
        {
            Settings = WidgetHelper.GetWidgetSettingsFromGuid(guid) as WidgetClockSettings;
            if (Settings == null)
                return;
            var clockTimer = new DispatcherTimer {Interval = Settings.TickInterval};
            clockTimer.Tick += delegate { CurrentTime = DateTime.Now; };
            clockTimer.Start();
        }

        public WidgetClockSettings Settings { get; }

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