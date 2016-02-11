using System.Linq;
using System.Windows;
using DesktopWidgets.Classes;
using DesktopWidgets.Windows;

namespace DesktopWidgets.Helpers
{
    public static class EventActionHelper
    {
        public static void NewPair()
        {
            var dialog = new SelectDualItem(EventActionFactory.AvailableEvents, EventActionFactory.AvailableActions,
                "New Event and Action");
            dialog.ShowDialog();

            if (dialog.SelectedItem1 == null || dialog.SelectedItem2 == null)
                return;

            App.WidgetsSettingsStore.EventActionPairs.Add(new EventActionPair
            {
                Event = EventActionFactory.GetNewEventFromName((string) dialog.SelectedItem1),
                Action = EventActionFactory.GetNewActionFromName((string) dialog.SelectedItem2)
            });
        }

        public static void EditPair(EventActionId id)
        {
            var pair = App.WidgetsSettingsStore.EventActionPairs.FirstOrDefault(x => x.Identifier.Guid == id.Guid);
            if (pair == null)
                return;

            var editDialog = new EventActionPairEditor(pair);
            editDialog.ShowDialog();
        }

        public static void RemovePair(EventActionId id)
        {
            if (
                Popup.Show("Are you sure you want to delete this event and action pair?", MessageBoxButton.YesNo,
                    MessageBoxImage.Warning, MessageBoxResult.No) == MessageBoxResult.No)
                return;
            foreach (
                var pair in App.WidgetsSettingsStore.EventActionPairs.Where(x => x.Identifier.Guid == id.Guid).ToList())
                App.WidgetsSettingsStore.EventActionPairs.Remove(pair);
        }
    }
}