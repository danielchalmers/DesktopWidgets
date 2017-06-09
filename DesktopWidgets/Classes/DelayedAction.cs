using System;
using System.Windows.Threading;

namespace DesktopWidgets.Classes
{
    public static class DelayedAction
    {
        public static void RunAction(int delay, Action action)
        {
            if (delay <= 0)
            {
                action?.Invoke();
                return;
            }
            var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(delay) };
            timer.Tick += (sender, args) =>
            {
                action?.Invoke();
                timer?.Stop();
                timer = null;
            };
            timer.Start();
        }
    }
}