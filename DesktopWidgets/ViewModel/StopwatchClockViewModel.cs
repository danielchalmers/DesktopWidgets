using System;
using System.Windows.Input;
using DesktopWidgets.Commands;

namespace DesktopWidgets.ViewModel
{
    public class StopwatchClockViewModel : ClockViewModel
    {
        private bool _isRunning;

        private DateTime _startTime;

        public StopwatchClockViewModel(Guid guid) : base(guid)
        {
            Settings = WidgetHelper.GetWidgetSettingsFromGuid(guid) as WidgetStopwatchClockSettings;
            if (Settings == null)
                return;
            StartStopCommand = new DelegateCommand(StartStop);
            StartTime = DateTime.Now;
            CurrentTime = DateTime.Now;
            Stop();
        }

        public WidgetStopwatchClockSettings Settings { get; }
        public ICommand StartStopCommand { get; set; }

        public bool IsRunning
        {
            get { return _isRunning; }
            set
            {
                if (_isRunning != value)
                {
                    _isRunning = value;
                    RaisePropertyChanged(nameof(IsRunning));
                }
            }
        }

        public DateTime StartTime
        {
            get { return _startTime; }
            set
            {
                if (_startTime != value)
                {
                    _startTime = value;
                    RaisePropertyChanged(nameof(StartTime));
                }
            }
        }

        private void StartStop(object parameter = null)
        {
            if (IsRunning)
                Stop();
            else
                Start();
        }

        private void Start()
        {
            CurrentTime = DateTime.Now;
            StartTime = DateTime.Now;
            StartClockUpdateTimer();
            IsRunning = true;
        }

        private void Stop()
        {
            IsRunning = false;
            StopClockUpdateTimer();
        }
    }
}