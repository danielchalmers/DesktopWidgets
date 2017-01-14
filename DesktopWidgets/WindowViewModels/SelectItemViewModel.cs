using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace DesktopWidgets.WindowViewModels
{
    public class SelectItemViewModel : ViewModelBase
    {
        private object _selectedItem;

        public SelectItemViewModel(IEnumerable<object> items)
        {
            ItemsList = new ObservableCollection<object>(items);

            if (ItemsList.Count > 0)
            {
                SelectedItem = ItemsList[0];
            }
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
                    RaisePropertyChanged();
                }
            }
        }
    }
}