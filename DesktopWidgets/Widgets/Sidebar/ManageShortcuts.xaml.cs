#region

using System.Windows;
using System.Windows.Input;

#endregion

namespace DesktopWidgets.Widgets.Sidebar
{
    /// <summary>
    ///     Interaction logic for ManageShortcuts.xaml
    /// </summary>
    public partial class ManageShortcuts : Window
    {
        private readonly ViewModel _viewModel;

        public ManageShortcuts(ViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            lsShortcuts.ItemsSource = viewModel.Settings.Shortcuts;
        }

        private Shortcut SelectedShortcut => (Shortcut) lsShortcuts.SelectedItem;

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void listBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            lsShortcuts.UnselectAll();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.New();
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Remove(SelectedShortcut, true);
        }

        private void btnOptions_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.OpenProperties(SelectedShortcut);
        }

        private void btnMoveBrowse_Click(object sender, RoutedEventArgs e)
        {
            SelectedShortcut.OpenFolder();
        }

        private void btnMoveUp_Click(object sender, RoutedEventArgs e)
        {
            lsShortcuts.SelectedItem = _viewModel.MoveUp(SelectedShortcut);
        }

        private void btnMoveDown_Click(object sender, RoutedEventArgs e)
        {
            lsShortcuts.SelectedItem = _viewModel.MoveDown(SelectedShortcut);
        }

        private void btnMoveTop_Click(object sender, RoutedEventArgs e)
        {
            lsShortcuts.SelectedItem = _viewModel.MoveUp(SelectedShortcut, true);
        }

        private void btnMoveBottom_Click(object sender, RoutedEventArgs e)
        {
            lsShortcuts.SelectedItem = _viewModel.MoveDown(SelectedShortcut, true);
        }
    }
}