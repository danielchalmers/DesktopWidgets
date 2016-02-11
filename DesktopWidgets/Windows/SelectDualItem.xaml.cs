using System.Collections.Generic;
using System.Windows;
using DesktopWidgets.WindowViewModels;

namespace DesktopWidgets.Windows
{
    /// <summary>
    ///     Interaction logic for SelectDualItem.xaml
    /// </summary>
    public partial class SelectDualItem : Window
    {
        private readonly SelectDualItemViewModel _viewModel;

        public SelectDualItem(IEnumerable<object> items1, IEnumerable<object> items2, string title)
        {
            InitializeComponent();
            _viewModel = new SelectDualItemViewModel(items1, items2);
            Title = $"Select {title}";
            DataContext = _viewModel;
        }

        public object SelectedItem1 { get; private set; }
        public object SelectedItem2 { get; private set; }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            SelectedItem1 = _viewModel.SelectedItem1;
            SelectedItem2 = _viewModel.SelectedItem2;
            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}