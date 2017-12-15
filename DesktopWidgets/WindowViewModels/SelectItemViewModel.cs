using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;

namespace DesktopWidgets.WindowViewModels
{
    public class SelectItemViewModel : ViewModelBase
    {
        private object _selectedItem;

        public SelectItemViewModel(IEnumerable<object> items)
        {
            ItemsList = new ObservableCollection<object>(items);
            SelectedItem = ItemsList.FirstOrDefault();
        }

        public ObservableCollection<object> ItemsList { get; set; }

        public object SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    RaisePropertyChanged();
                }
            }
        }
    }
}