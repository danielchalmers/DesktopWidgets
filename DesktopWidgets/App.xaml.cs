using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
        private static bool SuccessfullyLoaded;
        public static HelperWindow HelperWindow;
        public static TaskbarIcon TrayIcon;
        public static WidgetsSettingsStore WidgetsSettingsStore;
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

            if (WidgetsSettingsStore == null)
                WidgetsSettingsStore = new WidgetsSettingsStore {Widgets = new ObservableCollection<WidgetSettings>()};

            foreach (var settings in WidgetsSettingsStore.Widgets.Where(x => !x.Disabled))
            {
                var widgetView = new WidgetView(settings.Guid);
                var userControlStyle = (Style) widgetView.FindResource("UserControlStyle");
                UserControl userControl = null;
                object dataContext = null;

                if (settings is Widgets.TimeClock.Settings)
                {
                    dataContext = new Widgets.TimeClock.ViewModel(settings.Guid);
                    userControl = new Widgets.TimeClock.ControlView();
                }
                if (settings is Widgets.CountdownClock.Settings)
                {
                    dataContext = new Widgets.CountdownClock.ViewModel(settings.Guid);
                    userControl = new Widgets.CountdownClock.ControlView();
                }
                if (settings is Widgets.StopwatchClock.Settings)
                {
                    dataContext = new Widgets.StopwatchClock.ViewModel(settings.Guid);
                    userControl = new Widgets.StopwatchClock.ControlView();
                }

                userControl.Style = userControlStyle;
                widgetView.DataContext = dataContext;
                widgetView.MainContentContainer.Child = userControl;

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

        private void menuItemManageWidgets_OnClick(object sender, RoutedEventArgs e)
        {
            new ManageWidgets().Show();
        }

        private void menuItemOptions_OnClick(object sender, RoutedEventArgs e)
        {
            new Options().ShowDialog();
        }

        private void menuItemExit_OnClick(object sender, RoutedEventArgs e)
        {
            Current.Shutdown();
        }
    }
}