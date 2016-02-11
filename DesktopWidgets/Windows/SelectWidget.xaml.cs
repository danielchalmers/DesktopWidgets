using System.Windows;
using DesktopWidgets.WidgetBase.Settings;

namespace DesktopWidgets.Windows
{
    /// <summary>
    ///     Interaction logic for SelectWidget.xaml
    /// </summary>
    public partial class SelectWidget : Window
    {
        public SelectWidget()
        {
            InitializeComponent();
        }

        public WidgetSettingsBase SelectedItem { get; private set; }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            SelectedItem = lsWidgets.SelectedItem as WidgetSettingsBase;
            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}