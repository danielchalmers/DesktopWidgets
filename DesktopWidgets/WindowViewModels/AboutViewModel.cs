using GalaSoft.MvvmLight;

namespace DesktopWidgets.WindowViewModels
{
    public class AboutViewModel : ViewModelBase
    {
        public AboutViewModel(string title, string text)
        {
            Title = title;
            Text = text;
        }

        public string Title { get; }
        public string Text { get; }
    }
}