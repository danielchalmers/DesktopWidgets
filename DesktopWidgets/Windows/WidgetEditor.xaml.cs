using System.Windows;

namespace DesktopWidgets.Windows
{
    /// <summary>
    ///     Interaction logic for WidgetEditor.xaml
    /// </summary>
    public partial class WidgetEditor : Window
    {
        public WidgetEditor(string name, object settings)
        {
            InitializeComponent();
            Title = $"Edit {name}";
            PropertyGrid.SelectedObject = settings;
        }

        private void btnOK_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}