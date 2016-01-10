#region

using System.Windows;
using System.Windows.Controls;
using DesktopWidgets.Helpers;

#endregion

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
            SettingsHelper.ImportData();
        }

        private void btnExport_OnClick(object sender, RoutedEventArgs e)
        {
            SettingsHelper.ExportData();
        }

        private void btnDefaults_OnClick(object sender, RoutedEventArgs e)
        {
            SettingsHelper.ResetSettings();
        }
    }
}