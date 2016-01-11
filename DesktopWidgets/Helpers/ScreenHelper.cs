#region

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using DesktopWidgets.Properties;

#endregion

namespace DesktopWidgets.Helpers
{
    internal static class ScreenHelper
    {
        private static Rect ToRect(this Screen screen)
        {
            return (Settings.Default.IgnoreAppBars ? screen.Bounds : screen.WorkingArea).ToRect();
        }

        public static Rect GetScreenBounds(string deviceName)
        {
            return GetScreen(deviceName).ToRect();
        }

        public static Screen GetScreen(string deviceName)
        {
            return
                Screen.AllScreens.FirstOrDefault(x => x.DeviceName.Contains(deviceName, true)) ??
                Screen.AllScreens.FirstOrDefault(x => x.Primary);
        }

        public static IEnumerable<Rect> GetAllScreenBounds()
        {
            return Screen.AllScreens.Select(ToRect);
        }
    }
}