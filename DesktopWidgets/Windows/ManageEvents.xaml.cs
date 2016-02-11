using System.Windows;

namespace DesktopWidgets.Windows
{
    /// <summary>
    ///     Interaction logic for ManageEvents.xaml
    /// </summary>
    public partial class ManageEvents : Window
    {
        public ManageEvents()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}