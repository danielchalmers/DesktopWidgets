using System.Windows.Controls;
using DesktopWidgets.WindowViewModels;

namespace DesktopWidgets.OptionsPages
{
    /// <summary>
    ///     Interaction logic for Changelog.xaml
    /// </summary>
    public partial class Changelog : Page
    {
        public Changelog(ChangelogViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}