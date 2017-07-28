using DesktopWidgets.Properties;
using Microsoft.Win32;

namespace DesktopWidgets.Helpers
{
    public static class RegistryHelper
    {
        public static void SetRunOnStartup(bool runOnStartup)
        {
            var registryKey = Registry.CurrentUser.OpenSubKey
                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (runOnStartup)
            {
                registryKey?.SetValue(Resources.AppName, AppHelper.AppPath + " -systemstartup");
            }
            else
            {
                registryKey?.DeleteValue(Resources.AppName, false);
            }
        }
    }
}