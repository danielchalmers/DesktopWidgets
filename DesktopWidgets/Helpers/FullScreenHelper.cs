using System.Linq;
using System.Windows.Forms;
using DesktopWidgets.Properties;

namespace DesktopWidgets.Helpers
{
    internal class FullScreenHelper
    {
        public static bool DoesMonitorHaveFullscreenApp(Screen screen)
        {
            return !Settings.Default.IgnoreFullscreenWindows &&
                   Win32Helper.GetForegroundApp()
                       .IsFullScreen(screen);
        }

        public static bool DoesMonitorHaveFullscreenApp(int index)
        {
            return DoesMonitorHaveFullscreenApp(index != -1
                ? Screen.AllScreens[index]
                : Screen.PrimaryScreen);
        }

        public static bool DoesAnyMonitorHaveFullscreenApp()
        {
            return Screen.AllScreens.Any(DoesMonitorHaveFullscreenApp);
        }
    }
}