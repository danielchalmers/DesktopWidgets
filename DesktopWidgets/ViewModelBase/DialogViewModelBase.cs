using System.Windows;
using System.Windows.Input;
using DesktopWidgets.Commands;

namespace DesktopWidgets.ViewModelBase
{
    public class DialogViewModelBase : ViewModelBase
    {
        public DialogViewModelBase()
        {
            Close = new DelegateCommand(CloseExecute);
        }

        public ICommand Close { get; set; }

        private static void CloseExecute(object parameter)
        {
            var window = parameter as Window;
            window?.Close();
        }
    }
}