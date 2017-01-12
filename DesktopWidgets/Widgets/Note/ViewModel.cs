using DesktopWidgets.Helpers;
using DesktopWidgets.WidgetBase;
using DesktopWidgets.WidgetBase.ViewModel;

namespace DesktopWidgets.Widgets.Note
{
    public class ViewModel : WidgetViewModelBase
    {
        public ViewModel(WidgetId id) : base(id)
        {
            Settings = id.GetSettings() as Settings;
            if (Settings == null)
            {
            }
        }

        public Settings Settings { get; }

        public string Text
        {
            get { return Settings.Text; }
            set
            {
                if (Settings.Text != value)
                {
                    Settings.Text = value;
                    RaisePropertyChanged();
                }
            }
        }
    }
}