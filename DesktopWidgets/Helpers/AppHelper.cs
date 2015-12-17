using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using DesktopWidgets.Properties;

namespace DesktopWidgets.Helpers
{
    internal static class AppHelper
    {
        public static readonly string AppPath =
            UpdateHelper.IsUpdateable
                ? $"\"{Environment.GetFolderPath(Environment.SpecialFolder.Programs)}\\Daniel Chalmers\\{Resources.AppName}.appref-ms\""
                : Application.ResourceAssembly.Location;

        public static void RestartApplication(IEnumerable<string> arguments)
        {
            var args = new List<string> {"restarting"};
            args.AddRange(arguments);
            Process.Start(AppPath, string.Join(",-", args));
            ShutdownApplication();
        }

        public static void ShutdownApplication()
        {
            Application.Current.Shutdown();
        }
    }
}