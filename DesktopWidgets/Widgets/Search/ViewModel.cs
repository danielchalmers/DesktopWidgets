using System.Diagnostics;
using System.Windows.Input;
using DesktopWidgets.Helpers;
using DesktopWidgets.WidgetBase;
using DesktopWidgets.WidgetBase.ViewModel;
using GalaSoft.MvvmLight.Command;

namespace DesktopWidgets.Widgets.Search
{
    public class ViewModel : WidgetViewModelBase
    {
        private string _searchText;

        public ViewModel(WidgetId id) : base(id)
        {
            Settings = id.GetSettings() as Settings;
            if (Settings == null)
                return;
            Go = new RelayCommand(GoExecute);
            OnKeyUp = new RelayCommand<KeyEventArgs>(OnKeyUpExecute);
        }

        public Settings Settings { get; }
        public ICommand Go { get; set; }
        public ICommand OnKeyUp { get; set; }

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    RaisePropertyChanged();
                }
            }
        }

        private void GoExecute()
        {
            var searchText = SearchText;
            SearchText = string.Empty;
            Process.Start($"{Settings.BaseUrl}{searchText}{Settings.URLSuffix}");

            OnSpecialEvent();
        }

        private void OnKeyUpExecute(KeyEventArgs args)
        {
            if (args.Key == Key.Enter)
                GoExecute();
        }
    }
}