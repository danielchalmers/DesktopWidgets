using System.Collections.Generic;
using System.Windows;
using DesktopWidgets.WindowViewModels;

namespace DesktopWidgets.Windows
{
    /// <summary>
    ///     Interaction logic for SelectItem.xaml
    /// </summary>
    public partial class SelectItem : Window
    {
        private readonly SelectItemViewModel _viewModel;

        public SelectItem(IEnumerable<object> items, string title)
        {
            InitializeComponent();
            _viewModel = new SelectItemViewModel(items);
            Title = $"Select {title}";
            DataContext = _viewModel;
        }

        public object SelectedItem { get; private set; }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            SelectedItem = _viewModel.SelectedItem;
            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}