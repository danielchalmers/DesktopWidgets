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
        public About()
        {
            InitializeComponent();

            txtDescription.Text = string.Format(Properties.Resources.About, AssemblyInfo.Title, AssemblyInfo.Version,
                Properties.Resources.GitHubIssues, Properties.Resources.GitHubCommits, AssemblyInfo.Copyright);
        }
    }
}