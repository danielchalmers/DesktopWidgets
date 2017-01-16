using System.Windows;
using DesktopWidgets.Events;
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

            foreach (var eventPair in App.WidgetsSettingsStore.EventActionPairs)
            {
                var evnt = eventPair.Event as TrayIconClickEvent;
                if (evnt == null || eventPair.Disabled || !evnt.DoubleClick)
                {
                    continue;
                }
                eventPair.Action.Execute();
            }
        }

        private void TrayIcon_OnTrayLeftMouseUp(object sender, RoutedEventArgs e)
        {
            UpdateHelper.HandleUpdate();

            foreach (var eventPair in App.WidgetsSettingsStore.EventActionPairs)
            {
                var evnt = eventPair.Event as TrayIconClickEvent;
                if (evnt == null || eventPair.Disabled || evnt.DoubleClick)
                {
                    continue;
                }
                eventPair.Action.Execute();
            }
        }

        private void TrayIcon_OnTrayBalloonTipClicked(object sender, RoutedEventArgs e)
        {
            UpdateHelper.HandleUpdate();
        }
    }
}