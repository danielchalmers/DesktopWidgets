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
            bounds.AddRange(
                App.WidgetViews.Where(x => !x.Settings.Disabled && !x.Equals(window)).Select(view => view.GetBounds()));
            var windowRect = window.GetBounds();

            var horizontal = new List<double>();
            horizontal.AddRange(bounds.Select(x => x.Left));
            horizontal.AddRange(bounds.Select(x => x.Right));
            var vertical = new List<double>();
            vertical.AddRange(bounds.Select(x => x.Top));
            vertical.AddRange(bounds.Select(x => x.Bottom));

            var newLeft = double.NaN;
            var newTop = double.NaN;

            foreach (var rect in horizontal.Distinct())
            {
                if (Math.Abs(rect - windowRect.Left) <= margin)
                    newLeft = rect;
                else if (Math.Abs(rect - windowRect.Right) <= margin)
                    newLeft = rect - windowRect.Width;
                else if (Math.Abs(rect - windowRect.Left) <= margin)
                    newLeft = rect;
                else if (Math.Abs(rect - windowRect.Right) <= margin)
                    newLeft = rect - windowRect.Width;

                if (!double.IsNaN(newLeft))
                {
                    window.Left = newLeft;
                    break;
                }
            }

            foreach (var rect in vertical.Distinct())
            {
                if (Math.Abs(rect - windowRect.Top) <= margin)
                    newTop = rect;
                else if (Math.Abs(rect - windowRect.Bottom) <= margin)
                    newTop = rect - windowRect.Height;
                else if (Math.Abs(rect - windowRect.Top) <= margin)
                    newTop = rect;
                else if (Math.Abs(rect - windowRect.Bottom) <= margin)
                    newTop = rect - windowRect.Height;

                if (!double.IsNaN(newTop))
                {
                    window.Top = newTop;
                    break;
                }
            }
        }
    }
}