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

        public static readonly string BackupDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Resources.AppName);

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
            if (!App.SuccessfullyLoaded)
            {
                return;
            }

            SaveWidgetsDataToSettings();

            Settings.Default.Save();

            if (Settings.Default.BackupInterval.TotalSeconds > 0 && DateTime.Now - Settings.Default.BackupInterval > Settings.Default.LastBackupDateTime)
            {
                Backup();
            }
        }

        public static void ResetSettings(bool msg = true)
        {
            if (msg && Popup.Show(
                "All options, widgets, events, and actions will be deleted.\n\n" +
                $"Are you sure you want to reset {AssemblyInfo.Title}?",
                MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No) == MessageBoxResult.No)
            {
                return;
            }
            Backup();
            Settings.Default.Reset();
            Settings.Default.MustUpgrade = false;
            Settings.Default.Widgets = string.Empty;
            LoadWidgetsDataFromSettings();
            WidgetHelper.LoadWidgetViews();
            if (msg)
            {
                Popup.Show(
                "All settings have been restored to default.\n\n" +
                $"Backup created in \"{BackupDirectory}\".");
            }
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

            // Backup old data before overwriting.
            Backup();

            // Replace widget store.
            Settings.Default.Widgets = data;
            Settings.Default.Save();
            LoadWidgetsDataFromSettings();
            WidgetHelper.LoadWidgetViews();
            return true;
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

            // Prompt user to confirm decision.
            if (Popup.Show(
                    "Are you sure you want to overwrite ALL widgets, events, and actions?",
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

            // Let user know the import worked.
            Popup.Show(
                "Import was successful.\n\n" +
                $"Backup created in \"{BackupDirectory}\".");
        }

        private static string GetExportData()
        {
            return JsonConvert.SerializeObject(App.WidgetsSettingsStore, JsonSerializerSettings);
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

            File.WriteAllText(dialog.FileName, GetExportData());
        }

        private static void Backup()
        {
            if (!App.SuccessfullyLoaded)
            {
                return;
            }

            Settings.Default.LastBackupDateTime = DateTime.Now;
            var filename = $"backup-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.txt";
            FileSystemHelper.WriteTextToFile(Path.Combine(BackupDirectory, filename), GetExportData());
        }

        public static void DeleteConfigFile(ConfigurationErrorsException configurationErrorsException)
        {
            File.Delete(((ConfigurationErrorsException)configurationErrorsException.InnerException).Filename);
        }
    }
}