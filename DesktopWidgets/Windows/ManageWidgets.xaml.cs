#region

using System.Windows;

#endregion

namespace DesktopWidgets.Windows
{
    /// <summary>
    ///     Interaction logic for ManageWidgets.xaml
    /// </summary>
    public partial class ManageWidgets : Window
    {
        public ManageWidgets()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}