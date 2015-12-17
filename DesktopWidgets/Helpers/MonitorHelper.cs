#region

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Forms;

#endregion

namespace DesktopWidgets.Helpers
{
    internal static class MonitorHelper
    {
        public static Rect GetMonitorBounds(int index)
        {
            if (index == -1 || Screen.AllScreens.Length < index)
                index = GetPrimaryIndex();
            return Screen.AllScreens[index].WorkingArea.ToRect();
        }

        public static IEnumerable<Rect> GetAllMonitorBounds()
        {
            return Screen.AllScreens.Select(x => x.WorkingArea.ToRect());
        }

        public static int GetPrimaryIndex()
        {
            return Screen.AllScreens.ToList().FindIndex(x => x.Primary);
        }
    }
}