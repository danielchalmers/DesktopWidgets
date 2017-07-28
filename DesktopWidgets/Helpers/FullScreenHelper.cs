using System.Linq;
using System.Windows;
using System.Windows.Forms;
using DesktopWidgets.Classes;

namespace DesktopWidgets.Helpers
{
    public static class FullScreenHelper
    {
        private static bool DoesMonitorHaveFullscreenApp(Screen screen) => Win32Helper.GetForegroundApp()
            .IsFullScreen(screen);

        private static bool DoesMonitorHaveFullscreenApp(Screen screen, Win32App ignoreApp)
        {
            var foregroundApp = Win32Helper.GetForegroundApp();
            return foregroundApp.Hwnd != ignoreApp?.Hwnd && foregroundApp.IsFullScreen(screen);
        }

        public static bool DoesMonitorHaveFullscreenApp(Rect bounds)
            => DoesMonitorHaveFullscreenApp(ScreenHelper.GetScreen(bounds));

        public static bool DoesMonitorHaveFullscreenApp(Rect bounds, Win32App ignoreApp)
            => DoesMonitorHaveFullscreenApp(ScreenHelper.GetScreen(bounds), ignoreApp);

        public static bool DoesAnyMonitorHaveFullscreenApp() => Screen.AllScreens.Any(DoesMonitorHaveFullscreenApp);
    }
}