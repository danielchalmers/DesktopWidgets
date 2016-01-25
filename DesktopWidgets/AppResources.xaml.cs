using System.Windows;
using DesktopWidgets.Helpers;

namespace DesktopWidgets
{
    partial class AppResources : ResourceDictionary
    {
        public AppResources()
        {
            InitializeComponent();
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
    }
}