using DesktopWidgets.Classes;
using DesktopWidgets.Helpers;
using DesktopWidgets.ViewModelBase;

namespace DesktopWidgets.Widgets.Notes
{
    public class ViewModel : WidgetViewModelBase
    {
        public ViewModel(WidgetId id) : base(id)
        {
            Settings = id.GetSettings() as Settings;
            if (Settings == null)
                return;
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
                    RaisePropertyChanged(nameof(Text));
                }
            }
        }
    }
}