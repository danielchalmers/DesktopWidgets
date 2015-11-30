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

        public ViewModel(WidgetId id) : base(id, false)
        {
            Settings = id.GetSettings() as Settings;
            if (Settings == null)
                return;
            StartStopCommand = new DelegateCommand(StartStop);
            StartTime = CurrentTime;
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
            StartTime = DateTime.Now;
            UpdateCurrentTime();
            IsRunning = true;
            StartClockUpdateTimer();
        }

        private void Stop()
        {
            IsRunning = false;
            StopClockUpdateTimer();
        }
    }
}