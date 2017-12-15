using System;
using System.ComponentModel;
using System.Windows;
using DesktopWidgets.Classes;
using DesktopWidgets.WidgetBase.Settings;
using Microsoft.Win32;

namespace DesktopWidgets.Windows
{
    /// <summary>
    ///     Interaction logic for WidgetPackageExport.xaml
    /// </summary>
    public partial class WidgetPackageExport : Window, INotifyPropertyChanged
    {
        private string _path;

        public WidgetPackageExport(WidgetSettingsBase settings)
        {
            InitializeComponent();

            if (string.IsNullOrWhiteSpace(Properties.Settings.Default.WidgetPackagePublisherName))
            {
                Properties.Settings.Default.WidgetPackagePublisherName = Environment.UserName;
            }

            Settings = settings;

            DataContext = this;
        }

        public WidgetSettingsBase Settings { get; set; }

        public string Path
        {
            get => _path;
            set
            {
                if (_path != value)
                {
                    _path = value;
                    RaisePropertyChanged(nameof(Path));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private void BrowsePath()
        {
            var dialog = new SaveFileDialog
            {
                Filter = Properties.Resources.PackageExtensionFilter,
                FileName = Settings.PackageInfo.Name
            };
            if (dialog.ShowDialog() != true)
            {
                return;
            }
            Path = dialog.FileName;
        }

        private void btnOK_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Path))
            {
                BrowsePath();
            }
            if (string.IsNullOrWhiteSpace(Settings.PackageInfo.Name))
            {
                Popup.Show("You must enter a widget name.", image: MessageBoxImage.Stop);
                return;
            }
            if (string.IsNullOrWhiteSpace(Properties.Settings.Default.WidgetPackagePublisherName))
            {
                Popup.Show("You must enter a publisher name.", image: MessageBoxImage.Stop);
                return;
            }
            Settings.PackageInfo.Publisher = Properties.Settings.Default.WidgetPackagePublisherName;
            DialogResult = true;
        }

        private void btnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void btnBrowse_OnClick(object sender, RoutedEventArgs e)
        {
            BrowsePath();
        }
    }
}