#region

using System.Linq;
using System.Windows;
using System.Windows.Forms;

#endregion

namespace DesktopWidgets.Helpers
{
    internal class MonitorHelper
    {
        public static Rect GetMonitorBounds(int index)
        {
            if (index == -1 || Screen.AllScreens.Length < index)
                index = GetPrimaryIndex();
            var workingArea = Screen.AllScreens[index].WorkingArea;
            return new Rect(workingArea.X, workingArea.Y, workingArea.Width, workingArea.Height);
        }

        public static int GetPrimaryIndex()
        {
            return Screen.AllScreens.ToList().FindIndex(x => x.Primary);
        }
    }
}