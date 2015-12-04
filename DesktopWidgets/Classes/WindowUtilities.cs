using System.Windows;

namespace DesktopWidgets.Classes
{
    public static class WindowUtilities
    {
        public static Rect GetBounds(this Window target)
        {
            return new Rect(target.Left, target.Top, target.ActualWidth, target.ActualHeight);
        }
    }
}