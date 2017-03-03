using System.Diagnostics;
using System.Windows.Input;
using DesktopWidgets.Helpers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace DesktopWidgets.WindowViewModels
{
    public class AboutViewModel : ViewModelBase
    {
        public AboutViewModel(string title, string text, bool showLicensesButton)
        {
            Title = title;
            Text = text;
            ShowLicensesButton = showLicensesButton;
            ViewLicenses = new RelayCommand(ViewLicensesExecute);
        }

        public string Title { get; }
        public string Text { get; }
        public ICommand ViewLicenses { get; }
        public bool ShowLicensesButton { get; set; }

        private void ViewLicensesExecute()
        {
            Process.Start(AboutHelper.LicensesDirectory);
        }
    }
}