using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using DesktopWidgets.Properties;

namespace DesktopWidgets.Helpers
{
    public static class SnapHelper
    {
        private static IEnumerable<Rect> GetSnapBounds(this Window window, bool useWidgetsBounds, bool useFullBounds)
        {
            foreach (var bounds in ScreenHelper.GetAllScreenBounds(useFullBounds))
                yield return bounds;
            if (useWidgetsBounds)
                foreach (var bounds in App.WidgetViews.Where(x => !x.Settings.Disabled && !x.Equals(window))
                    .Select(view => view.GetBounds()))
                    yield return bounds;
        }

        private static bool IsSnappable(double pos1, double pos2)
            => Math.Abs(pos1 - pos2) <= Settings.Default.SnapMargin;

        public static Point Snap(this Window window, bool useWidgetsBounds = false, bool useFullBounds = false)
        {
            var compareBounds = GetSnapBounds(window, useWidgetsBounds, useFullBounds);
            var windowBounds = window.GetBounds();
            var x = SnapHorizontally(windowBounds, compareBounds);
            var y = SnapVertically(windowBounds, compareBounds);
            return new Point(x, y);
        }

        private static double SnapHorizontally(Rect windowBounds,
            IEnumerable<Rect> compareBounds)
        {
            var horizontal = new List<double>();
            horizontal.AddRange(compareBounds.Select(x => x.Left));
            horizontal.AddRange(compareBounds.Select(x => x.Right));

            foreach (var pos in horizontal.Distinct())
            {
                if (IsSnappable(pos, windowBounds.Left))
                    return pos;
                if (IsSnappable(pos, windowBounds.Right))
                    return pos - windowBounds.Width;
            }
            return windowBounds.Left;
        }

        private static double SnapVertically(Rect windowBounds,
            IEnumerable<Rect> compareBounds)
        {
            var vertical = new List<double>();
            vertical.AddRange(compareBounds.Select(x => x.Top));
            vertical.AddRange(compareBounds.Select(x => x.Bottom));

            foreach (var pos in vertical.Distinct())
            {
                if (IsSnappable(pos, windowBounds.Top))
                    return pos;
                if (IsSnappable(pos, windowBounds.Bottom))
                    return pos - windowBounds.Height;
            }
            return windowBounds.Top;
        }
    }
}