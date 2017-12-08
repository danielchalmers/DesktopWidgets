using System.Windows.Controls;

namespace DesktopWidgets.Widgets.Sidebar
{
    /// <summary>
    ///     Interaction logic for ControlView.xaml
    /// </summary>
    public partial class ControlView : UserControl
    {
        public ControlView()
        {
            InitializeComponent();
        }

        private void SeparatorBorder_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Don't move sidebar while dragging seperator.
            e.Handled = true;
        }
    }
}