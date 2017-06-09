using System.Diagnostics;
using System.Windows.Input;
using DesktopWidgets.Helpers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace DesktopWidgets.WindowViewModels
{
    public class AboutViewModel : ViewModelBase
    {
        private string _text;

        public AboutViewModel(string title, string text)
        {
            Title = title;
            Text = text;
            ViewLicenses = new RelayCommand(ViewLicensesExecute);
        }

        public string Title { get; }

        public string Text
        {
            get { return _text; }
            set { Set(ref _text, value); }
        }

        public ICommand ViewLicenses { get; }

        private void ViewLicensesExecute()
        {
            Process.Start(AboutHelper.LicensesDirectory);
        }
    }
}