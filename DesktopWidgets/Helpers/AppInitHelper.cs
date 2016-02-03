using System;
using System.Threading;
using DesktopWidgets.Classes;
using DesktopWidgets.Properties;
using DesktopWidgets.Windows;

namespace DesktopWidgets.Helpers
{
    public static class AppInitHelper
    {
        public static bool Initialize()
        {
            App.HelperWindow = new HelperWindow();
            SettingsHelper.UpgradeSettings();
            SettingsHelper.LoadSettings();
            if (IsAppAlreadyRunning())
                return false;

            WidgetHelper.LoadWidgetViews(App.Arguments.Contains("-systemstartup"));

            StartScheduledTasks();

            App.SaveTimer = new SaveTimer(Settings.Default.SaveDelay);

            App.SuccessfullyLoaded = true;

            if (!App.Arguments.Contains("-systemstartup") && App.WidgetsSettingsStore.Widgets.Count == 0)
                new ManageWidgets().Show();

            CheckForUpdatesDelayed();

            return true;
        }

        private static bool IsAppAlreadyRunning()
        {
            bool isNewInstance;
            App.AppMutex = new Mutex(true, AssemblyInfo.Guid, out isNewInstance);
            if (App.Arguments.Contains("-restarting") || App.Arguments.Contains("-multiinstance") ||
                Settings.Default.AllowMultiInstance || isNewInstance)
            {
                return false;
            }
            SingleInstanceHelper.ShowFirstInstance();
            AppHelper.ShutdownApplication();
            return true;
        }

        private static void StartScheduledTasks()
        {
            if (Settings.Default.UpdateCheckIntervalMinutes > 0)
            {
                App.UpdateScheduler = new TaskScheduler();
                App.UpdateScheduler.ScheduleTask(() =>
                    UpdateHelper.CheckForUpdatesAsync(true),
                    Settings.Default.CheckForUpdates && UpdateHelper.IsUpdateable,
                    TimeSpan.FromMinutes(Settings.Default.UpdateCheckIntervalMinutes));
                App.UpdateScheduler.Start();
            }
        }

        private static void CheckForUpdatesDelayed()
        {
            DelayedAction.RunAction(15000, delegate
            {
                if ((DateTime.Now - Settings.Default.LastUpdateCheck).TotalMinutes >=
                    Settings.Default.UpdateCheckIntervalMinutes)
                    App.UpdateScheduler?.RunTick();
            });
        }
    }
}