using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace DesktopWidgets.ViewModelBase
{
    public class DialogViewModelBase : GalaSoft.MvvmLight.ViewModelBase
    {
        public DialogViewModelBase()
        {
            Close = new RelayCommand<Window>(CloseExecute);
        }

        public ICommand Close { get; set; }

        private static void CloseExecute(Window window)
        {
            window.Close();
        }
    }
}