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
            SettingsHelper.LoadSettings();
            if (IsAppAlreadyRunning())
            {
                return false;
            }

            StartScheduledTasks();

            App.SaveTimer = new SaveTimer(Settings.Default.SaveDelay, Settings.Default.AutoSaveInterval);

            ForegroundTracker.AddHook();

            App.SuccessfullyLoaded = true;
            return true;
        }

        public static void InitializeExtra()
        {
            WidgetHelper.LoadWidgetViews(App.Arguments.Contains("-systemstartup"));

            if (!App.Arguments.Contains("-systemstartup") && App.WidgetsSettingsStore.Widgets.Count == 0)
            {
                new ManageWidgets().Show();
            }

            CheckForUpdatesDelayed();

            foreach (var eventPair in App.WidgetsSettingsStore.EventActionPairs)
            {
                if (eventPair.Event is LaunchEvent evnt)
                {
                    if ((!evnt.SystemStartup || App.Arguments.Contains("-systemstartup")) &&
                        (evnt.Parameters.Count == 0 || !evnt.Parameters.Except(App.Arguments).Any()))
                    {
                        eventPair.Action.Execute();
                    }
                }

                if (eventPair.Event is HotkeyEvent hotkeyEvent)
                {
                    HotkeyStore.RegisterHotkey(hotkeyEvent.Hotkey, eventPair.Action.Execute);
                }
            }

            if (App.Arguments.Contains("show-backup-help"))
            {
                Popup.Show(
                    $"Backups may be available in {SettingsHelper.AppDocumentsDirectory}.\n\n" +
                    $"Restore them through \"Import\" in Options.");
            }
        }

        private static bool IsAppAlreadyRunning()
        {
            App.AppMutex = new Mutex(true, AssemblyInfo.Guid, out var isNewInstance);
            if (App.Arguments.Contains("-restarting") ||
                App.Arguments.Contains("-multiinstance") ||
                Settings.Default.AllowMultiInstance ||
                isNewInstance)
            {
                return false;
            }
            SingleInstanceHelper.ShowFirstInstance();
            AppHelper.ShutdownApplication();
            return true;
        }

        private static void StartScheduledTasks()
        {
            if (Settings.Default.UpdateCheckInterval.TotalMinutes > 0)
            {
                App.UpdateScheduler = new TaskScheduler();
                App.UpdateScheduler.ScheduleTask(() =>
                    UpdateHelper.CheckForUpdatesAsync(true),
                    Settings.Default.CheckForUpdates && UpdateHelper.IsUpdateable,
                    Settings.Default.UpdateCheckInterval);
                App.UpdateScheduler.Start();
            }
        }

        private static void CheckForUpdatesDelayed()
        {
            if (Settings.Default.CheckForUpdatesOnStartupDelay.TotalMilliseconds > 0)
            {
                DelayedAction.RunAction((int)Settings.Default.CheckForUpdatesOnStartupDelay.TotalMilliseconds, delegate
                {
                    if ((DateTime.Now - Settings.Default.LastUpdateCheck).TotalMinutes >= Settings.Default.UpdateCheckInterval.TotalMinutes)
                    {
                        App.UpdateScheduler?.RunTick();
                    }
                });
            }
        }
    }
}