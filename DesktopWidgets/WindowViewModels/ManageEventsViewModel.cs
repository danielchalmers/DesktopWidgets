using System.Windows.Input;
using DesktopWidgets.Classes;
using DesktopWidgets.Helpers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace DesktopWidgets.WindowViewModels
{
    public class ManageEventsViewModel : ViewModelBase
    {
        private EventActionPair _selectedPair;

        public ManageEventsViewModel()
        {
            DeselectAll = new RelayCommand(DeselectAllExecute);
            NewPair = new RelayCommand(NewPairExecute);
            EditPair = new RelayCommand(EditPairExecute);
            ToggleEnablePair = new RelayCommand(ToggleEnablePairExecute);
            RemovePair = new RelayCommand(RemovePairExecute);
            ClonePair = new RelayCommand(ClonePairExecute);
        }

        public EventActionPair SelectedPair
        {
            get { return _selectedPair; }
            set
            {
                if (_selectedPair != value)
                {
                    _selectedPair = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ICommand DeselectAll { get; private set; }
        public ICommand NewPair { get; private set; }
        public ICommand EditPair { get; private set; }
        public ICommand ToggleEnablePair { get; private set; }
        public ICommand RemovePair { get; private set; }
        public ICommand ClonePair { get; private set; }

        private void DeselectAllExecute()
        {
            SelectedPair = null;
        }

        private void NewPairExecute()
        {
            EventActionHelper.New();
        }

        private void EditPairExecute()
        {
            SelectedPair.Identifier.Edit();
        }

        private void ToggleEnablePairExecute()
        {
            SelectedPair.Identifier.ToggleEnableDisable();
            DeselectAllExecute();
        }

        private void RemovePairExecute()
        {
            SelectedPair.Identifier.Remove();
            DeselectAllExecute();
        }

        private void ClonePairExecute()
        {
            SelectedPair.Identifier.Clone();
            DeselectAllExecute();
        }
    }
}