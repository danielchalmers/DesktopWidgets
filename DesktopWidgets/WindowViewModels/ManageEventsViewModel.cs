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
            RemovePair = new RelayCommand(RemovePairExecute);
        }

        public EventActionPair SelectedPair
        {
            get { return _selectedPair; }
            set
            {
                if (_selectedPair != value)
                {
                    _selectedPair = value;
                    RaisePropertyChanged(nameof(SelectedPair));
                }
            }
        }

        public ICommand DeselectAll { get; private set; }
        public ICommand NewPair { get; private set; }
        public ICommand EditPair { get; private set; }
        public ICommand RemovePair { get; private set; }

        private void DeselectAllExecute()
        {
            SelectedPair = null;
        }

        private void NewPairExecute()
        {
            EventActionHelper.NewPair();
        }

        private void EditPairExecute()
        {
            EventActionHelper.EditPair(SelectedPair.Identifier);
        }

        private void RemovePairExecute()
        {
            EventActionHelper.RemovePair(SelectedPair.Identifier);
        }
    }
}