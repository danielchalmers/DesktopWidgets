﻿using System.Windows;
using System.Windows.Controls;
using DesktopWidgets.Helpers;

namespace DesktopWidgets.OptionsPages
{
    /// <summary>
    ///     Interaction logic for General.xaml
    /// </summary>
    public partial class General : Page
    {
        public General()
        {
            InitializeComponent();
        }

        private void btnImport_OnClick(object sender, RoutedEventArgs e)
        {
            SettingsHelper.ImportWithDialog();
        }

        private void btnExport_OnClick(object sender, RoutedEventArgs e)
        {
            SettingsHelper.ExportWithDialog();
        }

        private void btnReset_OnClick(object sender, RoutedEventArgs e)
        {
            SettingsHelper.ResetSettings();
        }
    }
}