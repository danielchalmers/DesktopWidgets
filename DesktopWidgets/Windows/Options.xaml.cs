#region

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
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

            Pages = new ObservableCollection<Page>();
            Pages.Add(new General());
            if (Settings.Default.EnableAdvancedMode)
                Pages.Add(new Advanced());
            Pages.Add(new About());

            Settings.Default.Save();

            DataContext = this;
        }

        public ObservableCollection<Page> Pages { get; }

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
                be.UpdateSource();
            Settings.Default.Save();
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.Reload();
            Close();
        }

        private void frame_LoadCompleted(object sender, NavigationEventArgs e)
        {
            var content = (sender as Frame).Content as FrameworkElement;
            if (content == null)
                return;
            content.Style = (Style) FindResource("OptionsStyle");
        }
    }
}