#region

using System;
using System.Windows.Threading;

#endregion

namespace DesktopWidgets.Classes
{
    public static class DelayedAction
    {
        public static void RunAction(int delay, Action action)
        {
            var timer = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(delay)};
            timer.Tick += (sender, args) =>
            {
                action();
                timer.Stop();
            };
            timer.Start();
        }
    }
}