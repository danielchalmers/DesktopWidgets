using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using DesktopWidgets.Classes;

namespace DesktopWidgets.Helpers
{
    public static class WindowHelper
    {
        public static void Snap(this Window window)
        {
            const int margin = 4;

            var bounds = new List<Rect> {SystemParameters.WorkArea};
            bounds.AddRange(MonitorHelper.GetAllMonitorBounds());
            bounds.AddRange(App.WidgetViews.Where(x => !x.Equals(window)).Select(view => view.GetBounds()));

            var windowRect = window.GetBounds();
            foreach (var rect in bounds)
            {
                if (Math.Abs(rect.Left - windowRect.Left) <= margin)
                    window.Left = rect.Left;
                else if (Math.Abs(rect.Right - windowRect.Left) <= margin)
                    window.Left = rect.Right;
                else if (Math.Abs(rect.Left - windowRect.Right) <= margin)
                    window.Left = rect.Left - window.ActualWidth;
                else if (Math.Abs(windowRect.Left + windowRect.Width - rect.Left - rect.Width) <= margin)
                    window.Left = rect.Left + rect.Width - windowRect.Width;

                if (Math.Abs(rect.Top - windowRect.Top) <= margin)
                    window.Top = rect.Top;
                else if (Math.Abs(rect.Bottom - windowRect.Top) <= margin)
                    window.Top = rect.Bottom;
                else if (Math.Abs(rect.Top - windowRect.Bottom) <= margin)
                    window.Top = rect.Top - window.ActualHeight;
                else if (Math.Abs(windowRect.Top + windowRect.Height - rect.Top - rect.Height) <= margin)
                    window.Top = rect.Top + rect.Height - windowRect.Height;
            }
        }
    }
}