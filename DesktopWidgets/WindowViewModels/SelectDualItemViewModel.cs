using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace DesktopWidgets.WindowViewModels
{
    public class SelectDualItemViewModel : ViewModelBase
    {
        private object _selectedItem1;
        private object _selectedItem2;

        public SelectDualItemViewModel(IEnumerable<object> items1, IEnumerable<object> items2, string title1,
            string title2)
        {
            Title1 = title1;
            Title2 = title2;
            ItemsList1 = new ObservableCollection<object>(items1);
            ItemsList2 = new ObservableCollection<object>(items2);
        }

        public ObservableCollection<object> ItemsList1 { get; set; }
        public ObservableCollection<object> ItemsList2 { get; set; }
        public string Title1 { get; set; }
        public string Title2 { get; set; }

        public object SelectedItem1
        {
            get { return _selectedItem1; }
            set
            {
                if (_selectedItem1 != value)
                {
                    _selectedItem1 = value;
                    RaisePropertyChanged();
                }
            }
        }

        public object SelectedItem2
        {
            get { return _selectedItem2; }
            set
            {
                if (_selectedItem2 != value)
                {
                    _selectedItem2 = value;
                    RaisePropertyChanged();
                }
            }
        }
    }
}