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
            else
                Arguments = e.Args.ToList();

            if (!AppInitHelper.Initialize())
                return;
            TrayIcon = (TaskbarIcon) Resources["TrayIcon"];

            SystemEvents.SessionEnding += SystemEvents_OnSessionEnding;
            SystemEvents.DisplaySettingsChanged += (sender, args) => WidgetHelper.RefreshWidgets();

            UpdateHelper.HandleUpdate();
        }

        private void SystemEvents_OnSessionEnding(object sender, SessionEndingEventArgs e)
        {
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
                    : $"A critical exception occurred:\n\n{exception}\n\nApplication will now exit.",
                MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
            if (!SuccessfullyLoaded)
                AppHelper.ShutdownApplication();
        }
    }
}