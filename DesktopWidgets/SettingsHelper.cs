﻿#region

using System.Windows;
using DesktopWidgets.Properties;
using Newtonsoft.Json;

#endregion

namespace DesktopWidgets
{
    internal static class SettingsHelper
    {
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

        public static void LoadSettings()
        {
            App.WidgetCfg = JsonConvert.DeserializeObject<WidgetConfig>(Settings.Default.Widgets,
                new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.All});
        }

        public static void SaveSettings()
        {
            Settings.Default.Widgets = JsonConvert.SerializeObject(App.WidgetCfg,
                new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.All});

            Settings.Default.Save();
        }

        public static void ResetSettings(bool msg = true)
        {
            if (msg && Popup.Show(
                "Are you sure you want to reset ALL settings?\n\nThis cannot be undone.",
                MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No) == MessageBoxResult.No)
                return;
            Settings.Default.Reset();
            Settings.Default.MustUpgrade = false;
            if (msg)
                Popup.Show("All settings have been restored to default.", MessageBoxButton.OK,
                    MessageBoxImage.Information);
        }
    }
}