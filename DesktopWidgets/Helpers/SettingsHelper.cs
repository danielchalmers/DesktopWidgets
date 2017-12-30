using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Windows;
using DesktopWidgets.Classes;
using DesktopWidgets.Properties;
using DesktopWidgets.WidgetBase;
using DesktopWidgets.WidgetBase.Settings;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace DesktopWidgets.Helpers
{
    public static class SettingsHelper
    {
        public static readonly string AppDocumentsDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Resources.AppName);

        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
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
            return JsonConvert.DeserializeObject(JsonConvert.SerializeObject(obj, JsonSerializerSettingsAllTypeHandling), JsonSerializerSettingsAllTypeHandling);
        }

        public static void LoadSettings()
        {
            UpgradeSettingsIfRequired();

            LoadWidgetStoreFromSettings();
        }

        public static void SaveSettings()
        {
            if (!App.SuccessfullyLoaded)
            {
                return;
            }

            var serializedWidgetStore = GetSerializedWidgetStore();

            if (ShouldBackup())
            {
                Backup(serializedWidgetStore);
            }

            Settings.Default.Widgets = serializedWidgetStore;

            Settings.Default.Save();
        }

        public static void ResetSettings()
        {
            if (Popup.Show(
                "This will delete all options, widgets, events, and actions!\n\n" +
                $"Are you sure you want to reset {AssemblyInfo.Title}?",
                MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No) == MessageBoxResult.No)
            {
                return;
            }

            Backup();

            Settings.Default.Reset();
            Settings.Default.MustUpgrade = false;
            Settings.Default.Widgets = string.Empty;

            LoadWidgetStoreFromSettings();

            WidgetHelper.LoadWidgetViews();

            Popup.Show(
                "All settings have been restored to default.\n\n" +
                $"Backup created in \"{AppDocumentsDirectory}\".");
        }

        private static void UpgradeSettingsIfRequired()
        {
            if (Settings.Default.MustUpgrade)
            {
                Settings.Default.Upgrade();
                Settings.Default.MustUpgrade = false;
                Settings.Default.Save();
            }
        }

        private static void LoadWidgetStoreFromSettings()
        {
            App.WidgetsSettingsStore = GetDeserializedWidgetStoreFromSettings() ?? new WidgetsSettingsStore { Widgets = new ObservableCollection<WidgetSettingsBase>() };
            App.WidgetsSettingsStore.Widgets.CollectionChanged += (sender, args) => App.SaveTimer.DelaySave();
            App.WidgetsSettingsStore.EventActionPairs.CollectionChanged += (sender, args) => App.SaveTimer.DelaySave();
        }

        private static string GetSerializedWidgetStore()
        {
            return JsonConvert.SerializeObject(App.WidgetsSettingsStore, JsonSerializerSettings);
        }

        private static WidgetsSettingsStore GetDeserializedWidgetStoreFromSettings()
        {
            return JsonConvert.DeserializeObject<WidgetsSettingsStore>(Settings.Default.Widgets, JsonSerializerSettings);
        }

        public static void ExportWithDialog()
        {
            var dialog = new SaveFileDialog
            {
                Filter = Resources.StoreExportExtensionFilter,
                FileName = $"{Resources.AppName} export {DateTime.Now.ToString("yyMMddHHmmss")}"
            };

            if (dialog.ShowDialog() != true)
            {
                return;
            }

            File.WriteAllText(dialog.FileName, GetSerializedWidgetStore());
        }

        public static void ImportWithDialog()
        {
            var dialog = new OpenFileDialog
            {
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = Resources.StoreExportExtensionFilter
            };

            if (dialog.ShowDialog() != true)
            {
                return;
            }

            if (Popup.Show(
                    "This will overwrite all widgets, events, and actions!\n\n" +
                    $"Are you sure you want to import \"{dialog.FileName}\"?",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning,
                    MessageBoxResult.Yes)
                    != MessageBoxResult.Yes)
            {
                return;
            }

            var dataToImport = File.ReadAllText(dialog.FileName);
            if (!ImportData(dataToImport))
            {
                Popup.Show(
                    "Import failed.\n" +
                    "Data may be corrupt.\n\n" +
                    "No changes have been made.",
                    image: MessageBoxImage.Error);
            }

            Popup.Show(
                "Import was successful.\n\n" +
                $"Backup created in \"{AppDocumentsDirectory}\".");
        }

        private static bool ImportData(string data)
        {
            // Test new data before overwriting existing data.
            try
            {
                var newWidgets = JsonConvert.DeserializeObject<WidgetsSettingsStore>(data, JsonSerializerSettings);
                foreach (var widget in newWidgets.Widgets)
                {
                    var id = widget.Identifier.Guid;
                }
            }
            catch
            {
                return false;
            }

            // Backup before overwriting.
            Backup();

            // Replace widget store.
            Settings.Default.Widgets = data;
            Settings.Default.Save();

            // Load new widget store.
            LoadWidgetStoreFromSettings();
            WidgetHelper.LoadWidgetViews();

            return true;
        }

        private static bool ShouldBackup() => Settings.Default.BackupInterval.TotalSeconds > 0 && DateTime.Now - Settings.Default.BackupInterval > Settings.Default.LastBackupDateTime;

        private static void Backup(string serializedWidgetStore = null)
        {
            if (!App.SuccessfullyLoaded)
            {
                return;
            }

            Settings.Default.LastBackupDateTime = DateTime.Now;
            var filename = $"backup-{DateTime.Now.ToString("yyMMddHHmmss")}{Resources.StoreExportExtension}";
            FileSystemHelper.WriteTextToFile(Path.Combine(AppDocumentsDirectory, filename), serializedWidgetStore ?? GetSerializedWidgetStore());
        }

        public static void DeleteConfigFile(ConfigurationErrorsException configurationErrorsException)
        {
            File.Delete(((ConfigurationErrorsException)configurationErrorsException.InnerException).Filename);
        }
    }
}