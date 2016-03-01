using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using DesktopWidgets.Classes;
using DesktopWidgets.Events;
using DesktopWidgets.Properties;
using DesktopWidgets.View;
using DesktopWidgets.WidgetBase;
using DesktopWidgets.WidgetBase.Settings;
using DesktopWidgets.Windows;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace DesktopWidgets.Helpers
{
    public static class WidgetHelper
    {
        public static WidgetSettingsBase GetSettings(this WidgetId id)
        {
            return App.WidgetsSettingsStore.Widgets.FirstOrDefault(v => v.Identifier.Guid == id.Guid);
        }

        public static WidgetView GetView(this WidgetId id)
        {
            return App.WidgetViews.FirstOrDefault(w => w.Id?.Guid == id?.Guid && !w.IsClosed);
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
                new SelectItem(WidgetFactory.AvailableWidgets, "New Widget");
            dialog.ShowDialog();
            if (dialog.SelectedItem == null)
                return;
            AddNewWidget((string) dialog.SelectedItem);
        }

        private static void LoadView(this WidgetId id, bool systemStartup = false)
        {
            WidgetView widgetView = null;
            try
            {
                foreach (var view in App.WidgetViews.Where(view => view.Id == id).ToList())
                    view.Close();

                widgetView = new WidgetView(id, id.GetNewViewModel(), id.GetNewControlView(), systemStartup);
                App.WidgetViews.Add(widgetView);
                widgetView.Show();
            }
            catch (Exception ex)
            {
                widgetView?.Close();
                var name = id.GetName();
                Popup.ShowAsync($"{name} failed to load.\n\n{ex.Message}", image: MessageBoxImage.Error);
            }
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

        public static void Disable(this WidgetId id)
        {
            var settings = id.GetSettings();
            if (settings == null || settings.Disabled)
                return;
            settings.Disabled = true;
            id.CloseView();

            foreach (var eventPair in App.WidgetsSettingsStore.EventActionPairs)
            {
                var evnt = eventPair.Event as WidgetDisableEvent;
                if (evnt == null || eventPair.Disabled || evnt.WidgetId?.Guid != id?.Guid)
                    continue;
                eventPair.Action.Execute();
            }
        }

        public static void Enable(this WidgetId id)
        {
            var settings = id.GetSettings();
            if (settings == null || !settings.Disabled)
                return;
            settings.Disabled = false;
            id.LoadView();

            foreach (var eventPair in App.WidgetsSettingsStore.EventActionPairs)
            {
                var evnt = eventPair.Event as WidgetEnableEvent;
                if (evnt == null || eventPair.Disabled || evnt.WidgetId?.Guid != id?.Guid)
                    continue;
                eventPair.Action.Execute();
            }
        }

        public static void CloseView(this WidgetId id, bool reload = false)
        {
            try
            {
                var view = id.GetView();
                if (view == null)
                    return;
                if (reload)
                    view.CloseAction = () => { id.LoadView(); };
                view.CloseAnimation();
            }
            catch (Exception ex)
            {
                var name = id.GetName();
                Popup.ShowAsync($"{name} failed to close.\n\n{ex.Message}", image: MessageBoxImage.Error);
            }
        }

        public static void ToggleEnable(this WidgetId id)
        {
            var settings = id.GetSettings();
            if (settings == null)
                return;
            if (settings.Disabled)
                id.Enable();
            else
                id.Disable();
        }

        public static void Reload(this WidgetId id)
        {
            var settings = id.GetSettings();
            if (settings == null || settings.Disabled)
                return;
            id.CloseView(true);

            foreach (var eventPair in App.WidgetsSettingsStore.EventActionPairs)
            {
                var evnt = eventPair.Event as WidgetReloadEvent;
                if (evnt == null || eventPair.Disabled || evnt.WidgetId?.Guid != id?.Guid)
                    continue;
                eventPair.Action.Execute();
            }
        }

        public static void Remove(this WidgetId id, bool msg = false)
        {
            var settings = id.GetSettings();
            if (settings == null)
                return;
            if (msg && Popup.Show($"Are you sure you want to delete \"{settings.Name}\"?\n\nThis cannot be undone.",
                MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No) == MessageBoxResult.No)
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
            if (settings == null)
                return;
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
                foreach (var view in App.WidgetViews.Where(x => !x.IsClosed).ToList())
                    view?.Close();
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

        private static WidgetSettingsBase Deserialise(string settingsData)
        {
            try
            {
                return JsonConvert.DeserializeObject<WidgetSettingsBase>(settingsData,
                    SettingsHelper.JsonSerializerSettingsAllTypeHandling);
            }
            catch
            {
                // ignored
            }
            return null;
        }

        private static string Serialise(WidgetSettingsBase settings)
        {
            return JsonConvert.SerializeObject(settings, SettingsHelper.JsonSerializerSettingsAllTypeHandling);
        }

        private static string GetImportConfirmText(WidgetPackageInfo packageInfo)
        {
            var importText = new StringBuilder();
            importText.AppendLine("Do you want to import this widget?");
            importText.AppendLine();
            importText.AppendLine($"Publisher: {packageInfo.Publisher}");
            importText.AppendLine($"Name: {packageInfo.Name}");
            importText.AppendLine($"Publish Date: {packageInfo.PublishDateTime}");
            return importText.ToString();
        }

        public static void Import()
        {
            var dialog = new OpenFileDialog
            {
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = Resources.PackageExtensionFilter,
                Multiselect = true
            };
            if (dialog.ShowDialog() != true)
                return;
            foreach (var fileName in dialog.FileNames)
            {
                var fileContent = File.ReadAllText(fileName);

                WidgetSettingsBase settings = null;
                try
                {
                    settings = Deserialise(CompressionHelper.Decompress(fileContent));
                }
                catch
                {
                    // ignored
                }

                if (settings?.PackageInfo == null)
                {
                    Popup.Show("Import failed. Widget may be corrupt.", image: MessageBoxImage.Error);
                    return;
                }

                if (
                    Popup.Show(GetImportConfirmText(settings.PackageInfo), MessageBoxButton.YesNo,
                        MessageBoxImage.Question) ==
                    MessageBoxResult.No)
                    return;

                if (settings.PackageInfo.AppVersion > AssemblyInfo.Version)
                {
                    Popup.Show(
                        $"This widget requires a higher version.\n\nSupported version: {settings.PackageInfo.AppVersion}\nCurrent version: {AssemblyInfo.Version}",
                        image: MessageBoxImage.Error);
                    return;
                }

                settings.Identifier.GenerateNewGuid();
                settings.Disabled = false;
                AddNewWidget(settings);
            }
        }

        public static void Export(WidgetSettingsBase widget)
        {
            var settings = SettingsHelper.CloneObject(widget) as WidgetSettingsBase;
            if (settings == null)
                return;
            settings.PackageInfo = new WidgetPackageInfo {Name = settings.Name};
            var dialog = new WidgetPackageExport(settings);
            if (dialog.ShowDialog() != true)
                return;
            File.WriteAllText(dialog.Path, CompressionHelper.Compress(Serialise(settings)));
            Popup.Show($"\"{settings.PackageInfo.Name}\" has been saved to \"{dialog.Path}\".");
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

        public static WidgetId ChooseWidget()
        {
            var dialog = new SelectWidget();
            dialog.ShowDialog();
            return dialog.SelectedItem?.Identifier;
        }
    }
}