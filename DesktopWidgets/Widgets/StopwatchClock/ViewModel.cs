using System;
using System.Windows.Input;
using DesktopWidgets.Classes;
using DesktopWidgets.Commands;
using DesktopWidgets.Helpers;
using DesktopWidgets.ViewModelBase;

namespace DesktopWidgets.Widgets.StopwatchClock
{
    public class ViewModel : ClockViewModelBase
    {
        private bool _isRunning;

        private DateTime _startTime;

        public ViewModel(WidgetId id) : base(id)
        {
            Settings = WidgetHelper.GetWidgetSettingsFromId(id) as Settings;
            if (Settings == null)
                return;
            StartStopCommand = new DelegateCommand(StartStop);
            StartTime = DateTime.Now;
            CurrentTime = DateTime.Now;
            Stop();
        }

        public Settings Settings { get; }
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