using DesktopWidgets.Properties;
using Hardcodet.Wpf.TaskbarNotification;

namespace DesktopWidgets.Helpers
{
    internal static class TrayIconHelper
    {
        public static void ShowBalloon(string text, BalloonIcon icon)
        {
            App.TrayIcon.ShowBalloonTip(Resources.AppName, text, icon);
        }
    }
}