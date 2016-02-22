using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Deployment.Application;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using DesktopWidgets.Classes;
using DesktopWidgets.Helpers;
using DesktopWidgets.Properties;
using DesktopWidgets.Stores;
using DesktopWidgets.View;
using DesktopWidgets.WidgetBase;
using DesktopWidgets.Windows;
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
        public static HelperWindow HelperWindow;
        public static TaskbarIcon TrayIcon;
        public static WidgetsSettingsStore WidgetsSettingsStore;
        public static ObservableCollection<WidgetView> WidgetViews;
        public static SaveTimer SaveTimer;
        public static TaskScheduler UpdateScheduler;
        public static List<string> Arguments;
        public static bool IsWorkstationLocked;

        public App()
        {
            Dispatcher.UnhandledException += OnDispatcherUnhandledException;
        }

        public static bool IsMuted => Settings.Default.MuteEndTime > DateTime.Now;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (ApplicationDeployment.IsNetworkDeployed &&
                AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData != null &&
                AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData.Length > 0)
                Arguments =
                    AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData[0].Split(',').ToList();
            else if (e.Args.Length > 0)
                Arguments = e.Args[0].Split(',').ToList();
            else
                Arguments = new List<string>();

            if (!AppInitHelper.Initialize())
                return;
            TrayIcon = (TaskbarIcon) Resources["TrayIcon"];
            AppInitHelper.InitializeExtra();

            SystemEvents.SessionEnding += SystemEvents_OnSessionEnding;
            SystemEvents.SessionSwitch += SystemEventsOnSessionSwitch;
            SystemEvents.DisplaySettingsChanged += (sender, args) => WidgetHelper.RefreshWidgets();

            UpdateHelper.HandleUpdate();
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

        private void SystemEvents_OnSessionEnding(object sender, SessionEndingEventArgs e)
        {
            SettingsHelper.SaveSettings();
            AppHelper.ShutdownApplication();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            try
            {
                base.OnExit(e);

                SettingsHelper.SaveSettings();

                AppMutex?.ReleaseMutex();
            }
            catch
            {
                // ignored
            }
        }

        private static void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var exception = e.Exception.Message;
            Popup.Show(
                SuccessfullyLoaded
                    ? $"An unhandled exception occurred:\n\n{exception}"
                    : $"A critical unhandled exception occurred:\n\n{exception}\n\n{DesktopWidgets.Properties.Resources.AppName} will now exit.",
                MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
            if (!SuccessfullyLoaded)
                AppHelper.ShutdownApplication();
        }

        public static void Mute()
        {
            WidgetHelper.DismissWidgets();
            MediaPlayerStore.StopAll();
            Settings.Default.MuteEndTime = DateTime.Now + Settings.Default.MuteDuration;
        }

        public static void Unmute()
        {
            Settings.Default.MuteEndTime = DateTime.Now;
        }

        public static void ToggleMute()
        {
            if (IsMuted)
                Unmute();
            else
                Mute();
        }
    }
}