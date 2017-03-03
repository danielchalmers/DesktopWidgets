#region

using System.Windows.Controls;
using DesktopWidgets.WindowViewModels;

#endregion

namespace DesktopWidgets.OptionsPages
{
    /// <summary>
    ///     Interaction logic for About.xaml
    /// </summary>
    public partial class About : Page
    {
        public About(string title, string text)
        {
            InitializeComponent();
            DataContext = new AboutViewModel(title, text);
        }
    }
}