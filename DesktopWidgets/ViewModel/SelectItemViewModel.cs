using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DesktopWidgets.ViewModel
{
    public class SelectItemViewModel : GalaSoft.MvvmLight.ViewModelBase
    {
        private object _selectedItem;

        public SelectItemViewModel(IEnumerable<object> items)
        {
            ItemsList = new ObservableCollection<object>(items);
        }

        public ObservableCollection<object> ItemsList { get; set; }

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
    }
}