using System.Windows.Input;
using DesktopWidgets.Classes;
using DesktopWidgets.Helpers;
using GalaSoft.MvvmLight.Command;

namespace DesktopWidgets.ViewModel
{
    public class ManageWidgetsViewModel : GalaSoft.MvvmLight.ViewModelBase
    {
        private WidgetSettingsBase _selectedWidget;

        public ManageWidgetsViewModel()
        {
            DeselectAll = new RelayCommand(DeselectAllExecute);
            NewWidget = new RelayCommand(NewWidgetExecute);
            EditWidget = new RelayCommand(EditWidgetExecute);
            DisableWidget = new RelayCommand(DisableWidgetExecute);
            RemoveWidget = new RelayCommand(RemoveWidgetExecute);
        }

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

        public ICommand DeselectAll { get; private set; }

        public ICommand NewWidget { get; private set; }

        public ICommand EditWidget { get; private set; }

        public ICommand DisableWidget { get; private set; }

        public ICommand RemoveWidget { get; private set; }

        private void DeselectAllExecute()
        {
            SelectedWidget = null;
        }

        private void NewWidgetExecute()
        {
            WidgetHelper.NewWidget();
        }

        private void EditWidgetExecute()
        {
            SelectedWidget.Identifier.Edit();
        }

        private void DisableWidgetExecute()
        {
            SelectedWidget.Identifier.ToggleEnable();
            DeselectAllExecute();
        }

        private void RemoveWidgetExecute()
        {
            SelectedWidget.Identifier.Remove(true);
        }
    }
}