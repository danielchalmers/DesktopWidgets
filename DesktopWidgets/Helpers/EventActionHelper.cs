using System.Linq;
using System.Windows;
using DesktopWidgets.Classes;
using DesktopWidgets.Events;
using DesktopWidgets.Stores;
using DesktopWidgets.Windows;

namespace DesktopWidgets.Helpers
{
    public static class EventActionHelper
    {
        private static void Add(this EventActionPair pair)
        {
            App.WidgetsSettingsStore.EventActionPairs.Add(pair);
        }

        public static EventActionPair New()
        {
            var dialog = new SelectDualItem(
                EventActionFactory.AvailableEvents,
                EventActionFactory.AvailableActions,
                "New Event and Action Pair",
                "Event:",
                "Action:");
            dialog.ShowDialog();

            if (dialog.SelectedItem1 == null || dialog.SelectedItem2 == null)
            {
                return null;
            }

            var newPair = new EventActionPair
            {
                Event = EventActionFactory.GetNewEventFromName((string)dialog.SelectedItem1),
                Action = EventActionFactory.GetNewActionFromName((string)dialog.SelectedItem2)
            };
            newPair.Add();

            newPair.Edit();

            return newPair;
        }

        private static EventActionPair GetPair(this EventActionId id)
        {
            return App.WidgetsSettingsStore.EventActionPairs.FirstOrDefault(x => x?.Identifier?.Guid == id?.Guid);
        }

        public static void Edit(this EventActionId id)
        {
            id.GetPair()?.Edit();
        }

        private static void Edit(this EventActionPair pair)
        {
            var editDialog = new EventActionPairEditor(pair);
            editDialog.ShowDialog();
            pair?.Refresh();
        }

        public static EventActionPair MoveUp(this EventActionId id)
        {
            var pair = id.GetPair();
            return pair == null ? null : App.WidgetsSettingsStore.EventActionPairs.MoveUp(pair);
        }

        public static EventActionPair MoveDown(this EventActionId id)
        {
            var pair = id.GetPair();
            return pair == null ? null : App.WidgetsSettingsStore.EventActionPairs.MoveDown(pair);
        }

        public static void ToggleEnableDisable(this EventActionId id)
        {
            var pair = id.GetPair();
            if (pair == null)
            {
                return;
            }
            pair.Disabled = !pair.Disabled;
            pair.Refresh();
        }

        public static void Remove(this EventActionId id)
        {
            if (Popup.Show("Are you sure you want to delete this event and action pair?", MessageBoxButton.YesNo,
                    MessageBoxImage.Warning, MessageBoxResult.Yes) == MessageBoxResult.No)
            {
                return;
            }
            foreach (var pair in App.WidgetsSettingsStore.EventActionPairs.Where(x => x.Identifier.Guid == id.Guid).ToList())
            {
                App.WidgetsSettingsStore.EventActionPairs.Remove(pair);
                if (pair.Event is HotkeyEvent hotkeyEvent)
                {
                    HotkeyStore.RemoveHotkey(hotkeyEvent.Hotkey);
                }
            }
        }

        private static void Refresh(this EventActionPair pair)
        {
            if (pair.Event is HotkeyEvent hotkeyEvent)
            {
                hotkeyEvent.Hotkey.Disabled = pair.Disabled;
                HotkeyStore.RegisterHotkey(hotkeyEvent.Hotkey, pair.Action.Execute);
            }
        }

        public static EventActionPair Clone(this EventActionId id)
        {
            var newPair = SettingsHelper.CloneObject(id.GetPair()) as EventActionPair;
            if (newPair != null)
            {
                newPair.Identifier.GenerateNewGuid();
                newPair.Add();
            }
            return newPair;
        }
    }
}