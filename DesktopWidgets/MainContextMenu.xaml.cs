using System.Windows;
using DesktopWidgets.Helpers;
using DesktopWidgets.Properties;
using DesktopWidgets.Windows;

namespace DesktopWidgets
{
    partial class MainContextMenu : ResourceDictionary
    {
        public MainContextMenu()
        {
            InitializeComponent();
        }

        private void menuItemManageWidgets_OnClick(object sender, RoutedEventArgs e)
        {
            new ManageWidgets().Show();
        }

        private void menuItemOptions_OnClick(object sender, RoutedEventArgs e)
        {
            new Options().Show();
        }

        private void menuItemCheckForUpdates_OnClick(object sender, RoutedEventArgs e)
        {
            UpdateHelper.CheckForUpdatesAsync(false);
        }

        private void menuItemExit_OnClick(object sender, RoutedEventArgs e)
        {
            AppHelper.ShutdownApplication();
        }

        private void menuItemNewWidget_OnClick(object sender, RoutedEventArgs e)
        {
            WidgetHelper.NewWidget();
        }

        private void menuItemReloadWidgets_OnClick(object sender, RoutedEventArgs e)
        {
            WidgetHelper.ReloadWidgets();
        }

        private void menuItemMute_OnClick(object sender, RoutedEventArgs e)
        {
            App.ToggleMute(Settings.Default.MuteDuration);
        }

        private void menuItemShowWidgets_OnClick(object sender, RoutedEventArgs e)
        {
            WidgetHelper.ShowAllWidgetIntros();
        }

        private void menuItemDismissWidgets_OnClick(object sender, RoutedEventArgs e)
        {
            WidgetHelper.DismissWidgets();
        }

        private void menuItemManageEvents_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new ManageEvents();
            dialog.Show();
        }

        private void menuItemUnmuteWidgets_OnClick(object sender, RoutedEventArgs e)
        {
            WidgetHelper.UnmuteWidgets();
        }
    }
}