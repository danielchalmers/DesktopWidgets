using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;
using DesktopWidgets.Classes;
using DesktopWidgets.Helpers;
using DesktopWidgets.View;
using DesktopWidgets.Windows;
using Hardcodet.Wpf.TaskbarNotification;

namespace DesktopWidgets
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static bool SuccessfullyLoaded;
        public static HelperWindow HelperWindow;
        public static TaskbarIcon TrayIcon;
        public static WidgetsSettingsStore WidgetsSettingsStore;
        public static ObservableCollection<WidgetView> WidgetViews;
        public static SaveTimer SaveTimer;
        public static TaskScheduler UpdateScheduler;

        public App()
        {
            Dispatcher.UnhandledException += OnDispatcherUnhandledException;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            AppInitHelper.Initialize();
            TrayIcon = (TaskbarIcon) FindResource("TrayIcon");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            try
            {
                base.OnExit(e);

                SettingsHelper.SaveSettings();
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