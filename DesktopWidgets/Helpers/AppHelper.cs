using System;
using System.Collections.Generic;
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

        public static void RestartApplication(IEnumerable<string> arguments = null)
        {
            var args = new List<string> {"restarting"};
            if (arguments != null)
                args.AddRange(arguments);
            App.RestartArguments = args;
            ShutdownApplication();
        }

        public static void ShutdownApplication()
        {
            Application.Current.Shutdown();
        }
    }
}