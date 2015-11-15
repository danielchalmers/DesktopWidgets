using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using DesktopWidgets.Commands;

namespace DesktopWidgets.ViewModel
{
    public class SelectItemViewModel : ViewModelBase
    {
        private object _selectedItem;

        public SelectItemViewModel(IEnumerable<object> items)
        {
            ItemsList = new ObservableCollection<object>(items);
            OKCommand = new DelegateCommand(OK);
        }

        public ObservableCollection<object> ItemsList { get; set; }
        public ICommand OKCommand { get; set; }

        public object SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    RaisePropertyChanged(nameof(SelectedItem));
                }
            }
        }

        private void OK(object parameter)
        {
            (parameter as Window).Close();
        }
    }
}