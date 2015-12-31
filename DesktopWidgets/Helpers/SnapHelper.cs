using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using DesktopWidgets.Classes;
using DesktopWidgets.Properties;

namespace DesktopWidgets.Helpers
{
    public static class SnapHelper
    {
        private static List<Rect> GetSnapBounds(this Window window, bool useWidgetsBounds)
        {
            var bounds = new List<Rect>();
            bounds.AddRange(MonitorHelper.GetAllMonitorBounds());
            if (useWidgetsBounds)
                bounds.AddRange(
                    App.WidgetViews.Where(x => !x.Settings.Disabled && !x.Equals(window))
                        .Select(view => view.GetBounds()));
            return bounds;
        }

        public static bool IsSnappable(double pos1, double pos2)
            => Math.Abs(pos1 - pos2) <= Settings.Default.SnapMargin;

        public static void Snap(this Window window, bool useWidgetsBounds = false, Action leftSnapAction = null,
            Action rightSnapAction = null, Action topSnapAction = null, Action bottomSnapAction = null)
        {
            var compareBounds = GetSnapBounds(window, useWidgetsBounds);
            var windowBounds = window.GetBounds();
            window.SnapHorizontally(windowBounds, compareBounds, leftSnapAction, rightSnapAction);
            window.SnapVertically(windowBounds, compareBounds, topSnapAction, bottomSnapAction);
        }

        private static bool SnapHorizontally(this Window window, Rect windowBounds,
            IReadOnlyCollection<Rect> compareBounds, Action leftSnapAction, Action rightSnapAction)
        {
            var horizontal = new List<double>();
            horizontal.AddRange(compareBounds.Select(x => x.Left));
            horizontal.AddRange(compareBounds.Select(x => x.Right));

            foreach (var pos in horizontal.Distinct())
            {
                if (IsSnappable(pos, windowBounds.Left))
                {
                    window.Left = pos;
                    leftSnapAction?.Invoke();
                    return true;
                }
                if (IsSnappable(pos, windowBounds.Right))
                {
                    window.Left = pos - windowBounds.Width;
                    rightSnapAction?.Invoke();
                    return true;
                }
            }
            return false;
        }

        private static bool SnapVertically(this Window window, Rect windowBounds,
            IReadOnlyCollection<Rect> compareBounds, Action topSnapAction, Action bottomSnapAction)
        {
            var vertical = new List<double>();
            vertical.AddRange(compareBounds.Select(x => x.Top));
            vertical.AddRange(compareBounds.Select(x => x.Bottom));

            foreach (var pos in vertical.Distinct())
            {
                if (IsSnappable(pos, windowBounds.Top))
                {
                    window.Top = pos;
                    topSnapAction?.Invoke();
                    return true;
                }
                if (IsSnappable(pos, windowBounds.Bottom))
                {
                    window.Top = pos - windowBounds.Height;
                    bottomSnapAction?.Invoke();
                    return true;
                }
            }
            return false;
        }
    }
}