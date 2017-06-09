using System.Collections.Generic;
using System.Drawing;
using System.Windows;

namespace DesktopWidgets.Helpers
{
    public static class RectHelper
    {
        public static Rect ToRect(this Rectangle rectangle)
        {
            return new Rect(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }

        public static Rectangle ToRectangle(this Rect rect)
        {
            return new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
        }

        public static IEnumerable<Rect> GetCorners(this Rect rect, int size)
        {
            yield return new Rect(rect.Left, rect.Top, size, size);
            yield return new Rect(rect.Right - size, rect.Top, size, size);
            yield return new Rect(rect.Left, rect.Bottom - size, size, size);
            yield return new Rect(rect.Right - size, rect.Bottom - size, size, size);
        }
    }
}