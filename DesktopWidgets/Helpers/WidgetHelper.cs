using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using DesktopWidgets.Classes;
using DesktopWidgets.View;
using DesktopWidgets.WidgetBase;
using DesktopWidgets.WidgetBase.Settings;
using DesktopWidgets.Windows;

namespace DesktopWidgets.Helpers
{
    public static class WidgetHelper
    {
        public static WidgetSettingsBase GetSettings(this WidgetId id)
        {
            return App.WidgetsSettingsStore.Widgets.FirstOrDefault(v => v.Identifier == id);
        }

        private static WidgetView GetView(this WidgetId id)
        {
            return App.WidgetViews.FirstOrDefault(w => w.Id == id && !w.IsClosed);
        }

        public static string GetName(this WidgetId id)
        {
            var settings = id.GetSettings();
            var index = App.WidgetsSettingsStore.Widgets.IndexOf(settings);
            var name = settings.Name == "" ? $"Widget {index + 1}" : settings.Name;
            return $"{name} ({id.GetFriendlyName()})";
        }

        public static void NewWidget()
        {
            var dialog =
                new SelectItem(WidgetFactory.AvailableWidgets, "Widget");
            dialog.ShowDialog();
            if (dialog.SelectedItem == null)
                return;
            AddNewWidget((string) dialog.SelectedItem);
        }

        private static void LoadView(this WidgetId id, bool systemStartup = false)
        {
            foreach (var view in App.WidgetViews.Where(view => view.Id == id))
            {
                view.Close();
                App.WidgetViews.Remove(view);
            }

            var widgetView = new WidgetView(id, id.GetNewViewModel(), id.GetNewControlView(), systemStartup);
            App.WidgetViews.Add(widgetView);
            widgetView.Show();
        }

        private static void AddNewWidget(string type)
        {
            var newWidget = WidgetFactory.GetNewSettingsFromFriendlyName(type);
            App.WidgetsSettingsStore.Widgets.Add(newWidget);
            newWidget.Identifier.LoadView();
        }

        private static void AddNewWidget(WidgetSettingsBase settings)
        {
            App.WidgetsSettingsStore.Widgets.Add(settings);
            settings.Identifier.LoadView();
        }

        private static void Disable(this WidgetId id)
        {
            var settings = id.GetSettings();
            if (settings.Disabled)
                return;
            settings.Disabled = true;
            var view = id.GetView();
            if (view == null)
                return;
            view.Animate(AnimationMode.Hide, false, null, view.Close);
            App.WidgetViews.Remove(view);
        }

        private static void Enable(this WidgetId id)
        {
            var settings = id.GetSettings();
            if (!settings.Disabled)
                return;
            settings.Disabled = false;
            id.LoadView();
        }

        public static void ToggleEnable(this WidgetId id)
        {
            if (id.GetSettings().Disabled)
                id.Enable();
            else
                id.Disable();
        }

        public static void Reload(this WidgetId id)
        {
            if (id.GetSettings().Disabled)
                return;
            id.Disable();
            id.Enable();
        }

        public static void Remove(this WidgetId id, bool msg = false)
        {
            var settings = id.GetSettings();
            if (msg && Popup.Show($"Are you sure you want to delete \"{settings.Name}\"?\n\nThis cannot be undone.",
                MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes) == MessageBoxResult.No)
                return;

            App.WidgetsSettingsStore.Widgets.Remove(settings);
            var view = id.GetView();
            if (view != null)
            {
                view.Close();
                App.WidgetViews.Remove(view);
            }
        }

        public static void Edit(this WidgetId id, bool dismiss = false)
        {
            var settings = id.GetSettings();
            var view = id.GetView();
            var name = id.GetName();
            var previousHorizontalAlignment = settings.HorizontalAlignment;
            var previousVerticalAlignment = settings.VerticalAlignment;
            var previousIsDocked = settings.IsDocked;
            if (dismiss)
                view?.Dismiss();
            new WidgetEditor(name, settings).ShowDialog();
            if (view != null && !view.IsClosed)
                view.UpdateUi(isDocked: previousIsDocked, dockHorizontalAlignment: previousHorizontalAlignment,
                    dockVerticalAlignment: previousVerticalAlignment);
        }

        public static void LoadWidgetViews(bool systemStartup = false)
        {
            if (App.WidgetViews != null)
                foreach (var view in App.WidgetViews)
                    view.Close();
            App.WidgetViews = new ObservableCollection<WidgetView>();

            foreach (
                var id in
                    App.WidgetsSettingsStore.Widgets.Where(x => !x.Disabled).Select(settings => settings.Identifier))
                id.LoadView(systemStartup);
        }

        public static void ShowAllWidgetIntros()
        {
            foreach (var view in App.WidgetViews)
                view.ShowIntro();
        }

        public static void RefreshWidgets()
        {
            foreach (var view in App.WidgetViews)
                view.UpdateUi();
        }

        public static WidgetId Clone(this WidgetId id)
        {
            var newWidget = SettingsHelper.CloneObject(id.GetSettings()) as WidgetSettingsBase;
            if (newWidget != null)
            {
                newWidget.Identifier.GenerateNewGuid();
                AddNewWidget(newWidget);
            }
            return newWidget?.Identifier;
        }

        public static void Import(object widgetData)
        {
            var newWidget = SettingsHelper.CloneObject(widgetData) as WidgetSettingsBase;
            if (newWidget == null)
                return;
            newWidget.Identifier.GenerateNewGuid();
            AddNewWidget(newWidget);
        }

        public static void ReloadWidgets()
        {
            foreach (var id in App.WidgetViews.Select(x => x.Id).ToList())
                id.Reload();
        }

        public static void DismissWidgets()
        {
            foreach (var view in App.WidgetViews)
                view.Dismiss();
        }
    }
}