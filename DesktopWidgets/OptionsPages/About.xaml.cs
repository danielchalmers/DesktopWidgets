using System.Windows.Controls;
using DesktopWidgets.WindowViewModels;

namespace DesktopWidgets.OptionsPages
{
    /// <summary>
    ///     Interaction logic for About.xaml
    /// </summary>
    public partial class About : Page
    {
        public About(string title, string text, bool showLicensesButton)
        {
            InitializeComponent();
            DataContext = new AboutViewModel(title, text, showLicensesButton);
        }
    }
}