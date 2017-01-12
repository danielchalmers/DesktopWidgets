#region

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using DesktopWidgets.Classes;
using DesktopWidgets.Properties;
using DesktopWidgets.WidgetBase;
using DesktopWidgets.WidgetBase.Settings;
using DesktopWidgets.Windows;
using Newtonsoft.Json;

#endregion

namespace DesktopWidgets.Helpers
{
    internal static class SettingsHelper
    {
        public static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            ObjectCreationHandling = ObjectCreationHandling.Replace,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };

        public static readonly JsonSerializerSettings JsonSerializerSettingsAllTypeHandling = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            ObjectCreationHandling = ObjectCreationHandling.Replace,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };

        public static object CloneObject(object obj)
        {
            return
                JsonConvert.DeserializeObject(
                    JsonConvert.SerializeObject(obj, JsonSerializerSettingsAllTypeHandling),
                    JsonSerializerSettingsAllTypeHandling);
        }

        public static void UpgradeSettings()
        {
            // Upgrade settings from old version.
            if (Settings.Default.MustUpgrade)
            {
                Settings.Default.Upgrade();
                Settings.Default.MustUpgrade = false;
                Settings.Default.Save();
            }
        }

        private static void LoadWidgetsDataFromSettings()
        {
            App.WidgetsSettingsStore =
                JsonConvert.DeserializeObject<WidgetsSettingsStore>(Settings.Default.Widgets, JsonSerializerSettings) ??
                new WidgetsSettingsStore
                {
                    Widgets = new ObservableCollection<WidgetSettingsBase>()
                };
            App.WidgetsSettingsStore.Widgets.CollectionChanged += (sender, args) => App.SaveTimer.DelaySave();
            App.WidgetsSettingsStore.EventActionPairs.CollectionChanged += (sender, args) => App.SaveTimer.DelaySave();
        }

        private static void SaveWidgetsDataToSettings()
        {
            Settings.Default.Widgets = JsonConvert.SerializeObject(App.WidgetsSettingsStore, JsonSerializerSettings);
        }

        public static void LoadSettings()
        {
            LoadWidgetsDataFromSettings();
        }

        public static void SaveSettings()
        {
            SaveWidgetsDataToSettings();

            Settings.Default.Save();

            RegistryHelper.SetRunOnStartup(Settings.Default.RunOnStartup);
        }

        public static void ResetSettings(bool msg = true, bool refresh = true)
        {
            if (msg && Popup.Show(
                "Are you sure you want to reset ALL settings (including widgets)?\n\nThis cannot be undone.",
                MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No) == MessageBoxResult.No)
            {
                return;
            }
            Settings.Default.Reset();
            Settings.Default.MustUpgrade = false;
            Settings.Default.Widgets = string.Empty;
            LoadWidgetsDataFromSettings();
            if (refresh)
            {
                WidgetHelper.LoadWidgetViews();
            }
            if (msg)
            {
                Popup.Show("All settings have been restored to default.");
            }
        }

        public static void ImportData()
        {
            var dialog = new InputBox("Import Widgets");
            dialog.ShowDialog();
            if (dialog.Cancelled == false &&
                Popup.Show(
                    "Are you sure you want to overwrite all current widgets?\n\nThis cannot be undone.",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                try
                {
                    // Test import data before overwriting existing data.
                    foreach (
                        var id in
                            JsonConvert.DeserializeObject<WidgetsSettingsStore>(dialog.InputData, JsonSerializerSettings)
                                .Widgets.Select(x => x.Identifier.Guid))
                    {
                    }
                }
                catch
                {
                    Popup.Show(
                        $"Import failed. Data may be corrupt.{Environment.NewLine}{Environment.NewLine}No changes have been made.",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                Settings.Default.Widgets = dialog.InputData;
                Settings.Default.Save();
                LoadWidgetsDataFromSettings();
                WidgetHelper.LoadWidgetViews();
            }
        }

        public static void ExportData()
        {
            var dialog = new InputBox("Export Widgets",
                JsonConvert.SerializeObject(App.WidgetsSettingsStore, JsonSerializerSettings));
            dialog.ShowDialog();
        }
    }
}