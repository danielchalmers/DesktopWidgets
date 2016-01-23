using System.Windows;
using DesktopWidgets.Classes;
using DesktopWidgets.Helpers;

namespace DesktopWidgets.Windows
{
    /// <summary>
    ///     Interaction logic for EditWidget.xaml
    /// </summary>
    public partial class EditWidget : Window
    {
        public EditWidget(WidgetId id)
        {
            InitializeComponent();
            Title = $"Edit {id.GetName()}";
            PropertyGrid.SelectedObject = id.GetSettings();
        }

        private void btnOK_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}