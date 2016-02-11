using System.Windows;
using DesktopWidgets.Actions;
using DesktopWidgets.Classes;
using DesktopWidgets.Events;
using DesktopWidgets.Helpers;

namespace DesktopWidgets.Windows
{
    /// <summary>
    ///     Interaction logic for EventActionPairEditor.xaml
    /// </summary>
    public partial class EventActionPairEditor : Window
    {
        public EventActionPairEditor(EventActionPair pair)
        {
            InitializeComponent();
            EventActionPair = pair;
            DataContext = this;
        }

        public EventActionPair EventActionPair { get; set; }

        private void btnOK_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void btnSelectWidgetForEvent_OnClick(object sender, RoutedEventArgs e)
        {
            ((WidgetEventBase) EventActionPair.Event).WidgetId = WidgetHelper.ChooseWidget();
        }

        private void btnSelectWidgetForAction_OnClick(object sender, RoutedEventArgs e)
        {
            ((WidgetActionBase) EventActionPair.Action).WidgetId = WidgetHelper.ChooseWidget();
        }
    }
}