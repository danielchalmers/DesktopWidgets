using System;
using System.Linq;
using System.Threading;
using DesktopWidgets.Classes;
using DesktopWidgets.Events;
using DesktopWidgets.Properties;
using DesktopWidgets.Stores;
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

            ForegroundTracker.AddHook();

            App.SuccessfullyLoaded = true;

            if (!App.Arguments.Contains("-systemstartup") && App.WidgetsSettingsStore.Widgets.Count == 0)
                new ManageWidgets().Show();

            CheckForUpdatesDelayed();

            foreach (var eventPair in App.WidgetsSettingsStore.EventActionPairs)
            {
                var evnt = eventPair.Event as LaunchEvent;
                if (evnt != null)
                {
                    if ((!evnt.SystemStartup || App.Arguments.Contains("-systemstartup")) &&
                        (evnt.Parameters.Count == 0 || !evnt.Parameters.Except(App.Arguments).Any()))
                        eventPair.Action.Execute();
                }

                var hotkeyEvent = eventPair.Event as HotkeyEvent;
                if (hotkeyEvent != null)
                    HotkeyStore.RegisterHotkey(hotkeyEvent.Hotkey, eventPair.Action.Execute);
            }
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