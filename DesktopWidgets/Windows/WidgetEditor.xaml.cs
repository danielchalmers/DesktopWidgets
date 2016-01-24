using System.Windows;
using DesktopWidgets.Helpers;
using DesktopWidgets.WidgetBase;

namespace DesktopWidgets.Windows
{
    /// <summary>
    ///     Interaction logic for WidgetEditor.xaml
    /// </summary>
    public partial class WidgetEditor : Window
    {
        public WidgetEditor(WidgetId id)
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