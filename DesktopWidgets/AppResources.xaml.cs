using System;
using System.Windows;
using DesktopWidgets.Helpers;
using DesktopWidgets.Properties;
using DesktopWidgets.Windows;

namespace DesktopWidgets
{
    partial class AppResources : ResourceDictionary
    {
        public AppResources()
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

        private void TrayIcon_OnTrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            WidgetHelper.ShowAllWidgetIntros();
        }

        private void TrayIcon_OnTrayLeftMouseUp(object sender, RoutedEventArgs e)
        {
            UpdateHelper.HandleUpdate();
        }

        private void TrayIcon_OnTrayBalloonTipClicked(object sender, RoutedEventArgs e)
        {
            UpdateHelper.HandleUpdate();
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
            if (App.IsMuted)
            {
                Settings.Default.MuteEndTime = DateTime.Now;
            }
            else
            {
                WidgetHelper.HideWidgets();
                Settings.Default.MuteEndTime = DateTime.Now + Settings.Default.MuteDuration;
            }
        }

        private void menuItemShowWidgets_OnClick(object sender, RoutedEventArgs e)
        {
            WidgetHelper.ShowAllWidgetIntros();
        }
    }
}