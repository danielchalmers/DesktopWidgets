#region

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using DesktopWidgets.OptionsPages;
using DesktopWidgets.Properties;

#endregion

namespace DesktopWidgets
{
    /// <summary>
    ///     Interaction logic for Options.xaml
    /// </summary>
    public partial class Options : Window
    {
        private readonly List<Page> _pages;

        public Options()
        {
            InitializeComponent();

            _pages = new List<Page>();
            LoadPages();

            Settings.Default.Save();
        }

        private void LoadPages()
        {
            _pages.Add(new General());
            if (Settings.Default.EnableAdvancedMode)
                _pages.Add(new Advanced());
            _pages.Add(new About());
            for (var i = 0; i < _pages.Count; i++)
                NavBar.Items.Add(new ListBoxItem
                {
                    Content = _pages[i].Title,
                    Tag = i
                });
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            foreach (var be in _pages.SelectMany(BindingOperations.GetSourceUpdatingBindings))
                be.UpdateSource();
            Settings.Default.Save();
            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.Reload();
            DialogResult = true;
        }

        private void frame_LoadCompleted(object sender, NavigationEventArgs e)
        {
            var content = OptionsFrame.Content as FrameworkElement;
            if (content == null)
                return;
            content.Style = (Style) FindResource("OptionsStyle");
        }

        private void NavBar_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OptionsFrame.Navigate(_pages[NavBar.SelectedIndex]);
        }
    }
}