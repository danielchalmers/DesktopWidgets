using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using DesktopWidgets.UserControls;
using DesktopWidgets.View;
using DesktopWidgets.ViewModel;
using Hardcodet.Wpf.TaskbarNotification;

namespace DesktopWidgets
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static bool SuccessfullyLoaded;
        public static HelperWindow HelperWindow;
        public static TaskbarIcon TrayIcon;
        public static WidgetConfig WidgetCfg;
        public static ObservableCollection<WidgetView> WidgetViews;

        public App()
        {
            Dispatcher.UnhandledException += OnDispatcherUnhandledException;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            HelperWindow = new HelperWindow();
            SettingsHelper.UpgradeSettings();
            SettingsHelper.LoadSettings();
            TrayIcon = (TaskbarIcon) FindResource("TrayIcon");

            LoadWidgets();

            SuccessfullyLoaded = true;
        }

        private static void LoadWidgets()
        {
            if (WidgetViews != null)
                foreach (var view in WidgetViews)
                    view.Close();
            WidgetViews = new ObservableCollection<WidgetView>();

            if (WidgetCfg == null)
                WidgetCfg = new WidgetConfig {Widgets = new ObservableCollection<WidgetSettings>()};

            foreach (var settings in WidgetCfg.Widgets.Where(x => !x.Disabled))
            {
                var widgetView = new WidgetView();

                if (settings is WidgetClockSettings)
                {
                    widgetView.DataContext = new ClockViewModel(settings.Guid);
                    widgetView.MainContentContainer.Child = new Clock();
                }

                widgetView.Show();
                WidgetViews.Add(widgetView);
            }
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
                Current.Shutdown();
        }

        private void menuItemExit_OnClick(object sender, RoutedEventArgs e)
        {
            Current.Shutdown();
        }

        private void menuItemOptions_OnClick(object sender, RoutedEventArgs e)
        {
            new Options().ShowDialog();
        }
    }
}