using System.Windows.Input;
using DesktopWidgets.Helpers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace DesktopWidgets.WindowViewModels
{
    public class ChangelogViewModel : ViewModelBase
    {
        private string _text;

        public ChangelogViewModel(string title, string text)
        {
            Title = title;
            Text = text;
            CheckForUpdates = new RelayCommand(CheckForUpdatesExecute);
        }

        public string Title { get; }
        public string Text
        {
            get { return _text; }
            set { Set(ref _text, value); }
        }

        public ICommand CheckForUpdates { get; }

        private void CheckForUpdatesExecute()
        {
            UpdateHelper.CheckForUpdatesAsync(false);
        }
    }
}