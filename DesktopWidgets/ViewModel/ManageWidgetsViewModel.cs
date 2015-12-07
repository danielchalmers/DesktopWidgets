using System.Collections.ObjectModel;
using System.Windows.Input;
using DesktopWidgets.Classes;
using DesktopWidgets.Helpers;
using DesktopWidgets.ViewModelBase;
using GalaSoft.MvvmLight.Command;

namespace DesktopWidgets.ViewModel
{
    public class ManageWidgetsViewModel : DialogViewModelBase
    {
        private WidgetSettingsBase _selectedWidget;

        public ManageWidgetsViewModel()
        {
            WidgetList = App.WidgetsSettingsStore.Widgets;
            DeselectAllCommand = new RelayCommand(DeselectAll);
            NewWidgetCommand = new RelayCommand(NewWidget);
            EditWidgetCommand = new RelayCommand(EditWidget);
            DisableWidgetCommand = new RelayCommand(DisableWidget);
            RemoveWidgetCommand = new RelayCommand(RemoveWidget);
        }

        public ObservableCollection<WidgetSettingsBase> WidgetList { get; set; }

        public WidgetSettingsBase SelectedWidget
        {
            get { return _selectedWidget; }
            set
            {
                if (_selectedWidget != value)
                {
                    _selectedWidget = value;
                    RaisePropertyChanged(nameof(SelectedWidget));
                }
            }
        }

        public ICommand DeselectAllCommand { get; private set; }

        public ICommand NewWidgetCommand { get; private set; }

        public ICommand EditWidgetCommand { get; private set; }

        public ICommand DisableWidgetCommand { get; private set; }

        public ICommand RemoveWidgetCommand { get; private set; }

        private void DeselectAll()
        {
            SelectedWidget = null;
        }

        private void NewWidget()
        {
            WidgetHelper.NewWidget();
        }

        private void EditWidget()
        {
            SelectedWidget.Identifier.Edit();
        }

        private void DisableWidget()
        {
            SelectedWidget.Identifier.ToggleEnable();
            DeselectAll();
        }

        private void RemoveWidget()
        {
            SelectedWidget.Identifier.Remove(true);
        }
    }
}