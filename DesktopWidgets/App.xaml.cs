using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Deployment.Application;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using DesktopWidgets.Classes;
using DesktopWidgets.Events;
using DesktopWidgets.Helpers;
using DesktopWidgets.Properties;
using DesktopWidgets.Stores;
using DesktopWidgets.View;
using DesktopWidgets.WidgetBase;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Win32;

namespace DesktopWidgets
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static bool SuccessfullyLoaded;
        public static Mutex AppMutex;
        public static TaskbarIcon TrayIcon;
        public static WidgetsSettingsStore WidgetsSettingsStore;
        public static ObservableCollection<WidgetView> WidgetViews;
        public static SaveTimer SaveTimer;
        public static TaskScheduler UpdateScheduler;
        public static List<string> Arguments;
        public static bool IsWorkstationLocked;
        public static IEnumerable<string> RestartArguments;

        public App()
        {
            Dispatcher.UnhandledException += OnDispatcherUnhandledException;
        }

        public static bool IsMuted => Settings.Default.MuteEndTime > DateTime.Now;

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            if (ApplicationDeployment.IsNetworkDeployed &&
                AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData != null &&
                AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData.Length > 0)
            {
                Arguments =
                    AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData[0].Split(',').ToList();
            }
            else if (e.Args.Length > 0)
            {
                Arguments = e.Args[0].Split(',').ToList();
            }
            else
            {
                Arguments = new List<string>();
            }

            if (!AppInitHelper.Initialize())
            {
                return;
            }
            TrayIcon = (TaskbarIcon)Resources["TrayIcon"];
            AppInitHelper.InitializeExtra();

            SystemEvents.SessionSwitch += SystemEventsOnSessionSwitch;
            SystemEvents.DisplaySettingsChanged += (s, args) => WidgetHelper.RefreshWidgets();
            Settings.Default.PropertyChanged += Settings_OnPropertyChanged;

            UpdateHelper.HandleUpdate();
        }

        private void Settings_OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Settings.Default.RunOnStartup))
            {
                RegistryHelper.SetRunOnStartup(Settings.Default.RunOnStartup);
            }
        }

        private void SystemEventsOnSessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            switch (e.Reason)
            {
                case SessionSwitchReason.SessionLock:
                    IsWorkstationLocked = true;
                    break;
                case SessionSwitchReason.SessionUnlock:
                    IsWorkstationLocked = false;
                    break;
            }
        }

        private void App_OnExit(object sender, ExitEventArgs e)
        {
            try
            {
                SaveTimer?.Stop();

                SettingsHelper.SaveSettings();

                TrayIcon?.Dispose();

                AppMutex?.ReleaseMutex();
            }
            catch
            {
                // ignored
            }
            finally
            {
                if (RestartArguments != null)
                {
                    Process.Start(AppHelper.AppPath, string.Join(",", RestartArguments));
                }
            }
        }

        private static void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;

            ExceptionHelper.SaveException(e.Exception);

            if (SuccessfullyLoaded)
            {
                Popup.Show($"An exception occurred:\n{e.Exception.Message}", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (e.Exception is ConfigurationErrorsException configException)
                {
                    var revert = Popup.Show(
                        $"Failed to load settings with error:\n" +
                        $"\"{e.Exception.Message}\".\n\n" +
                        $"Attempt to revert to last known configuration?",
                        MessageBoxButton.YesNo, MessageBoxImage.Error, MessageBoxResult.Yes) == MessageBoxResult.Yes;

                    if (revert)
                    {
                        // Delete the bad user.config file and restart so a new one will be upgraded or regenerated.
                        SettingsHelper.DeleteConfigFile(configException);
                        AppHelper.RestartApplication(new List<string> { "show-backup-help" });
                    }
                    else
                    {
                        Popup.Show($"{DesktopWidgets.Properties.Resources.AppName} will now exit.", MessageBoxButton.OK, MessageBoxImage.Error);
                        AppHelper.ShutdownApplication();
                    }
                }
                else
                {
                    Popup.Show($"A critical exception occurred:\n" +
                        $"{e.Exception.Message}" +
                        $"{DesktopWidgets.Properties.Resources.AppName} will now exit.", MessageBoxButton.OK, MessageBoxImage.Error);
                    AppHelper.ShutdownApplication();
                }
            }
        }

        public static void Mute(TimeSpan duration)
        {
            Settings.Default.MuteEndTime = DateTime.Now + duration;
            MediaPlayerStore.StopAll();
            WidgetHelper.DismissWidgets();

            foreach (var eventPair in WidgetsSettingsStore.EventActionPairs)
            {
                var evnt = eventPair.Event as MuteUnmuteEvent;
                if (evnt == null || eventPair.Disabled ||
                    !(evnt.Mode == MuteEventMode.Both || evnt.Mode == MuteEventMode.Mute))
                {
                    continue;
                }
                eventPair.Action.Execute();
            }
        }

        public static void Unmute()
        {
            Settings.Default.MuteEndTime = DateTime.Now;

            foreach (var eventPair in WidgetsSettingsStore.EventActionPairs)
            {
                var evnt = eventPair.Event as MuteUnmuteEvent;
                if (evnt == null || eventPair.Disabled ||
                    !(evnt.Mode == MuteEventMode.Both || evnt.Mode == MuteEventMode.Unmute))
                {
                    continue;
                }
                eventPair.Action.Execute();
            }
        }

        public static void ToggleMute(TimeSpan duration)
        {
            if (IsMuted)
            {
                Unmute();
            }
            else
            {
                Mute(duration);
            }
        }
    }
}