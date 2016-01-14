#region

using System.Windows.Controls;

#endregion

namespace DesktopWidgets.OptionsPages
{
    /// <summary>
    ///     Interaction logic for About.xaml
    /// </summary>
    public partial class About : Page
    {
        public About(string title, string contents = "")
        {
            InitializeComponent();
            Title = title;
            txtAbout.Text = contents;
        }
    }
}