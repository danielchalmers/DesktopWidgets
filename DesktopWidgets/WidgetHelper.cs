using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using DesktopWidgets.View;

namespace DesktopWidgets
{
    public static class WidgetHelper
    {
        public static readonly List<string> WidgetNames = new List<string> {"Clock"};

        public static WidgetSettings GetWidgetSettingsFromGuid(Guid guid)
        {
            return App.WidgetCfg.Widgets.First(v => v.Guid == guid);
        }

        public static WidgetView GetWidgetViewFromGuid(Guid guid)
        {
            return App.WidgetViews.First(v => v.Guid == guid);
        }

        public static string GetWidgetName(Guid guid)
        {
            var settings = GetWidgetSettingsFromGuid(guid);
            var index = App.WidgetCfg.Widgets.IndexOf(settings);
            var name = (settings.Name == "" ? $"Widget {index + 1}" : settings.Name);
            return $"{name}";
        }

        public static void NewWidget()
        {
            var dialog = new SelectItem(WidgetNames);
            dialog.ShowDialog();
            AddWidget((string) dialog.SelectedItem);
        }

        public static void AddWidget(string type)
        {
            WidgetSettings newWidget;
            switch (type)
            {
                case "Clock":
                    newWidget = new WidgetClockSettings();
                    break;
                default:
                    newWidget = new WidgetSettings();
                    break;
            }
            App.WidgetCfg.Widgets.Add(newWidget);
        }

        public static void DisableWidget(Guid guid)
        {
            var settings = GetWidgetSettingsFromGuid(guid);
            if (settings.Disabled)
                return;
            //var view = GetWidgetViewFromGuid(guid);
            settings.Disabled = true;
            //view.Close();
            //App.WidgetViews.Remove(view);
        }

        public static void EnableWidget(Guid guid)
        {
            var settings = GetWidgetSettingsFromGuid(guid);
            if (!settings.Disabled)
                return;
            settings.Disabled = false;
        }

        public static void ToggleWidgetEnabled(Guid guid)
        {
            if (GetWidgetSettingsFromGuid(guid).Disabled)
                EnableWidget(guid);
            else
                DisableWidget(guid);
        }

        public static void RemoveWidget(Guid guid, bool msg = false)
        {
            if (msg && Popup.Show("Are you sure you want to delete this widget?\n\nThis cannot be undone.",
                MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes) == MessageBoxResult.No)
                return;

            var view = GetWidgetViewFromGuid(guid);
            var settings = GetWidgetSettingsFromGuid(guid);
            view.Close();
            App.WidgetCfg.Widgets.Remove(settings);
            App.WidgetViews.Remove(view);
        }

        public static void EditWidget(Guid guid)
        {
            new PropertyView(GetWidgetSettingsFromGuid(guid)).ShowDialog();
        }
    }
}