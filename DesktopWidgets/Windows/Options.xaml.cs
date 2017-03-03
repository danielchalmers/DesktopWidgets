#region

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using DesktopWidgets.Classes;
using DesktopWidgets.Helpers;
using DesktopWidgets.OptionsPages;
using DesktopWidgets.Properties;

#endregion

namespace DesktopWidgets.Windows
{
    /// <summary>
    ///     Interaction logic for Options.xaml
    /// </summary>
    public partial class Options : Window, INotifyPropertyChanged
    {
        private Page _currentPage;

        public Options()
        {
            InitializeComponent();

            Settings.Default.Save();
            DataContext = this;

            UpdateChangelog();
        }

        public IEnumerable<Page> Pages { get; } = new List<Page>
        {
            new General(),
            new About("Changelog", string.Empty),
            new About("About", AboutHelper.AboutText)
        };

        public Page CurrentPage
        {
            get { return _currentPage; }
            set
            {
                if (!Equals(_currentPage, value))
                {
                    _currentPage = value;
                    RaisePropertyChanged(nameof(CurrentPage));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            foreach (var be in Pages.SelectMany(BindingOperations.GetSourceUpdatingBindings))
            {
                be.UpdateSource();
            }
            Settings.Default.Save();
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.Reload();
            Close();
        }

        private void UpdateChangelog()
        {
            new ChangelogDownloader().GetChangelog(e =>
            {
                foreach (
                    var about in
                        Pages.OfType<About>().Where(x => x.Title == "Changelog"))
                {
                    about.txtAbout.Text = e;
                }
            });
        }
    }
}