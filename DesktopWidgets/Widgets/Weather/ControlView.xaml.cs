using System.Windows.Controls;
using DesktopWidgets.Helpers;

namespace DesktopWidgets.Widgets.Weather
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

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            ProcessHelper.Launch(e.Uri.ToString());
            e.Handled = true;
        }
    }
}