using System;
using DesktopWidgets.Helpers;
using DesktopWidgets.Properties;
using DesktopWidgets.Windows;
using Microsoft.Win32;

namespace DesktopWidgets.Classes
{
    public static class AppInitHelper
    {
        public static void Initialize()
        {
            App.HelperWindow = new HelperWindow();
            SettingsHelper.UpgradeSettings();
            SettingsHelper.LoadSettings();

            WidgetHelper.LoadWidgetViews();

            StartScheduledTasks();

            App.SaveTimer = new SaveTimer(Settings.Default.SaveDelay);
            SystemEvents.SessionEnding += (sender, args) => SettingsHelper.SaveSettings();

            App.SuccessfullyLoaded = true;

            CheckForUpdatesDelayed();
        }

        private static void StartScheduledTasks()
        {
            if (Settings.Default.UpdateCheckIntervalMinutes > 0)
            {
                App.UpdateScheduler = new TaskScheduler();
                App.UpdateScheduler.ScheduleTask(() =>
                    UpdateHelper.CheckForUpdatesAsync(true),
                    (Settings.Default.CheckForUpdates && UpdateHelper.IsUpdateable),
                    TimeSpan.FromMinutes(Settings.Default.UpdateCheckIntervalMinutes));
                App.UpdateScheduler.Start();
            }
        }

        private static void CheckForUpdatesDelayed()
        {
            DelayedAction.RunAction(15000, delegate
            {
                if ((DateTime.Now - Settings.Default.LastUpdateCheck).TotalDays > 30)
                    App.UpdateScheduler?.RunTick();
            });
        }
    }
}