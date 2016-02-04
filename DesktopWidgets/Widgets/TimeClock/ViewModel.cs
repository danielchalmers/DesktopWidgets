using System.Windows;
using System.Windows.Input;
using DesktopWidgets.Helpers;
using DesktopWidgets.WidgetBase;
using DesktopWidgets.WidgetBase.ViewModel;

namespace DesktopWidgets.Widgets.TimeClock
{
    public class ViewModel : ClockViewModelBase
    {
        public ViewModel(WidgetId id) : base(id)
        {
            Settings = id.GetSettings() as Settings;
            if (Settings == null)
                return;
        }

        public Settings Settings { get; }

        public override void LeftMouseDoubleClickExecute(MouseButtonEventArgs e)
        {
            base.LeftMouseDoubleClickExecute(e);
            if (Settings.CopyTextOnDoubleClick)
                Clipboard.SetText(CurrentTime.ParseCustomFormat(Settings.DateTimeFormat));
        }
    }
}