using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using DesktopWidgets.Helpers;
using DesktopWidgets.WidgetBase.Settings;

namespace DesktopWidgets.WidgetBase.ViewModel
{
    public abstract class ClockViewModelBase : WidgetViewModelBase
    {
        private readonly WidgetClockSettingsBase _settings;
        private DispatcherTimer _clockUpdateTimer;
        private DateTime _currentTime;

        public ClockViewModelBase(WidgetId id, bool startTicking = true) : base(id)
        {
            _settings = id.GetSettings() as WidgetClockSettingsBase;
            if (_settings == null)
            {
                return;
            }
            _clockUpdateTimer = new DispatcherTimer();
            _clockUpdateTimer.Tick += UpdateTimer_Tick;
            UpdateCurrentTime();
            if (startTicking)
            {
                StartClockUpdateTimer();
            }
        }

        public DateTime CurrentTime
        {
            get { return _currentTime; }
            set
            {
                if (_currentTime != value)
                {
                    _currentTime = value;
                    RaisePropertyChanged();
                }
            }
        }

        private void SyncClockUpdateInterval()
        {
            var newInterval = TimeSpan.FromMilliseconds(_settings.UpdateInterval > 0
                ? _settings.UpdateInterval
                : 1000 - DateTime.Now.Millisecond);
            if (_clockUpdateTimer.Interval != newInterval)
            {
                _clockUpdateTimer.Interval = newInterval;
                if (_clockUpdateTimer.IsEnabled)
                {
                    _clockUpdateTimer.Stop();
                    _clockUpdateTimer.Start();
                }
            }
        }

        public void UpdateCurrentTime()
        {
            CurrentTime = DateTime.Now + _settings.TimeOffset;
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

        public override void OnClose()
        {
            base.OnClose();
            _clockUpdateTimer?.Stop();
            _clockUpdateTimer = null;
        }

        public override void LeftMouseDoubleClickExecute(MouseButtonEventArgs e)
        {
            base.LeftMouseDoubleClickExecute(e);
            if (_settings.CopyTextOnDoubleClick)
            {
                var textBlock = View.GetMainElement() as TextBlock;
                if (textBlock != null)
                {
                    Clipboard.SetText(textBlock.Text);
                }
            }
        }

        protected virtual void UpdateTimer_Tick(object sender, EventArgs e)
        {
            UpdateCurrentTime();
        }
    }
}