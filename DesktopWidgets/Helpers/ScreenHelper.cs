#region

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

#endregion

namespace DesktopWidgets.Helpers
{
    internal static class ScreenHelper
    {
        public static Rect ToRect(this Screen screen, bool useFullBounds)
        {
            return (useFullBounds ? screen.Bounds : screen.WorkingArea).ToRect();
        }

        public static Rect GetScreenBounds(string deviceName, bool useFullBounds)
        {
            return GetScreen(deviceName).ToRect(useFullBounds);
        }

        public static Screen GetScreen(string deviceName)
        {
            return
                Screen.AllScreens.FirstOrDefault(x => x.DeviceName.Contains(deviceName, true)) ??
                Screen.AllScreens.FirstOrDefault(x => x.Primary);
        }

        public static Screen GetScreen(Rect bounds)
        {
            return
                Screen.AllScreens.FirstOrDefault(x => x.Bounds.IntersectsWith(bounds.ToRectangle())) ??
                Screen.AllScreens.FirstOrDefault(x => x.Primary);
        }

        public static IEnumerable<Rect> GetAllScreenBounds(bool useFullBounds)
        {
            return Screen.AllScreens.Select(x => x.ToRect(useFullBounds));
        }

        public static Screen GetScreen(Window window)
        {
            return Screen.FromHandle(new WindowInteropHelper(window).Handle);
        }
    }
}