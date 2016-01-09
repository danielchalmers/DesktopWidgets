#region

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using DesktopWidgets.Properties;

#endregion

namespace DesktopWidgets.Helpers
{
    internal static class MonitorHelper
    {
        private static Rect ScreenToRect(Screen screen)
        {
            return (Settings.Default.IgnoreAppBars ? screen.Bounds : screen.WorkingArea).ToRect();
        }

        public static Rect GetMonitorBounds(int index)
        {
            if (index == -1 || Screen.AllScreens.Length < index)
                index = GetPrimaryIndex();
            return ScreenToRect(Screen.AllScreens[index]);
        }

        public static IEnumerable<Rect> GetAllMonitorBounds()
        {
            return Screen.AllScreens.Select(ScreenToRect);
        }

        public static int GetPrimaryIndex()
        {
            return Screen.AllScreens.ToList().FindIndex(x => x.Primary);
        }
    }
}