using System.Windows;

namespace DesktopWidgets.Helpers
{
    internal class AppHelper
    {
        public static readonly string AppPath = Application.ResourceAssembly.Location;

        public static void ShutdownApplication()
        {
            Application.Current.Shutdown();
        }
    }
}