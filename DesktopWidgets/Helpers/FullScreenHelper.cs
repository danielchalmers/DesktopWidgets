using System.Linq;
using System.Windows.Forms;

namespace DesktopWidgets.Helpers
{
    internal static class FullScreenHelper
    {
        private static bool DoesMonitorHaveFullscreenApp(Screen screen) => Win32Helper.GetForegroundApp()
            .IsFullScreen(screen);

        public static bool DoesMonitorHaveFullscreenApp(string deviceName)
            => DoesMonitorHaveFullscreenApp(ScreenHelper.GetScreen(deviceName));

        public static bool DoesAnyMonitorHaveFullscreenApp() => Screen.AllScreens.Any(DoesMonitorHaveFullscreenApp);
    }
}