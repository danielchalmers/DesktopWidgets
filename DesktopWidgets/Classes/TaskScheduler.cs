#region

using System;
using System.Windows.Threading;

#endregion

namespace DesktopWidgets.Classes
{
    public class TaskScheduler
    {
        private readonly DispatcherTimer _taskTimer;
        private Action _action;
        private bool _condition;

        public TaskScheduler()
        {
            _taskTimer = new DispatcherTimer();
        }

        public void ScheduleTask(Action action, bool condition, TimeSpan interval)
        {
            _condition = condition;
            _action = action;
            _taskTimer.Tick += (sender, args) => RunTick();
            _taskTimer.Interval = interval;
        }

        public void Start() => _taskTimer.Start();
        public void Stop() => _taskTimer.Stop();

        public void RunTick()
        {
            if (_condition)
                _action?.Invoke();
        }
    }
}