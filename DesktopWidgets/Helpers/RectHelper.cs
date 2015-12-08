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
    }
}