﻿using System;
using System.Threading;
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
            if (IsAppAlreadyRunning())
                return;

            WidgetHelper.LoadWidgetViews();

            StartScheduledTasks();

            App.SaveTimer = new SaveTimer(Settings.Default.SaveDelay);
            SystemEvents.SessionEnding += (sender, args) => SettingsHelper.SaveSettings();

            App.SuccessfullyLoaded = true;

            CheckForUpdatesDelayed();
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