using GalaSoft.MvvmLight;

namespace DesktopWidgets.WindowViewModels
{
    public class ChangelogViewModel : ViewModelBase
    {
        private string _text;

        public ChangelogViewModel(string title, string text)
        {
            Title = title;
            Text = text;
        }

        public string Title { get; }
        public string Text
        {
            get { return _text; }
            set { Set(ref _text, value); }
        }
    }
}