using System.Windows;

namespace DesktopWidgets.Helpers
{
    public static class WindowHelper
    {
        public static Rect GetBounds(this Window target)
        {
            return new Rect(target.Left, target.Top, target.ActualWidth, target.ActualHeight);
        }
    }
}