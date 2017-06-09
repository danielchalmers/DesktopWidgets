using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DesktopWidgets.Widgets.Sidebar
{
    /// <summary>
    ///     Interaction logic for ManageShortcuts.xaml
    /// </summary>
    public partial class ManageShortcuts : Window, INotifyPropertyChanged
    {
        private Shortcut _selectedShortcut;

        public ManageShortcuts(ViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            DataContext = this;
        }

        public ViewModel ViewModel { get; }

        public Shortcut SelectedShortcut
        {
            get { return _selectedShortcut; }
            set
            {
                if (_selectedShortcut != value)
                {
                    _selectedShortcut = value;
                    RaisePropertyChanged(nameof(SelectedShortcut));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void listBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            (sender as ListBox)?.UnselectAll();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.New();
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Remove(SelectedShortcut, true);
        }

        private void btnOptions_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.OpenProperties(SelectedShortcut);
        }

        private void btnMoveBrowse_Click(object sender, RoutedEventArgs e)
        {
            SelectedShortcut.OpenFolder();
        }

        private void btnMoveUp_Click(object sender, RoutedEventArgs e)
        {
            SelectedShortcut = ViewModel.MoveUp(SelectedShortcut);
        }

        private void btnMoveDown_Click(object sender, RoutedEventArgs e)
        {
            SelectedShortcut = ViewModel.MoveDown(SelectedShortcut);
        }

        private void btnMoveTop_Click(object sender, RoutedEventArgs e)
        {
            SelectedShortcut = ViewModel.MoveUp(SelectedShortcut, true);
        }

        private void btnMoveBottom_Click(object sender, RoutedEventArgs e)
        {
            SelectedShortcut = ViewModel.MoveDown(SelectedShortcut, true);
        }

        protected void RaisePropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}