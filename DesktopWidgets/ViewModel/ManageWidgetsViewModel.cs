using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using DesktopWidgets.Commands;

namespace DesktopWidgets.ViewModel
{
    public class ManageWidgetsViewModel : ViewModelBase
    {
        private WidgetSettings _selectedWidget;

        public ManageWidgetsViewModel()
        {
            WidgetList = App.WidgetCfg.Widgets;
            CloseCommand = new DelegateCommand(Close);
            DeselectAllCommand = new DelegateCommand(DeselectAll);
            NewWidgetCommand = new DelegateCommand(NewWidget);
            EditWidgetCommand = new DelegateCommand(EditWidget);
            DisableWidgetCommand = new DelegateCommand(DisableWidget);
            RemoveWidgetCommand = new DelegateCommand(RemoveWidget);
        }

        public ObservableCollection<WidgetSettings> WidgetList { get; set; }

        public WidgetSettings SelectedWidget
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

        public ICommand CloseCommand { get; private set; }

        public ICommand DeselectAllCommand { get; private set; }

        public ICommand NewWidgetCommand { get; private set; }

        public ICommand EditWidgetCommand { get; private set; }

        public ICommand DisableWidgetCommand { get; private set; }

        public ICommand RemoveWidgetCommand { get; private set; }

        private void Close(object parameter)
        {
            (parameter as Window).Close();
        }

        private void DeselectAll(object parameter = null)
        {
            SelectedWidget = null;
        }

        private void NewWidget(object parameter)
        {
            WidgetHelper.NewWidget();
        }

        private void EditWidget(object parameter)
        {
            WidgetHelper.EditWidget(SelectedWidget.Guid);
        }

        private void DisableWidget(object parameter)
        {
            WidgetHelper.ToggleWidgetEnabled(SelectedWidget.Guid);
            DeselectAll();
        }

        private void RemoveWidget(object parameter)
        {
            WidgetHelper.RemoveWidget(SelectedWidget.Guid, true);
        }
    }
}