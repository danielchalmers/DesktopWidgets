using System.Windows;
using DesktopWidgets.Classes;

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
            EventPropertyGrid.SelectedObject = pair.Event;
            ActionPropertyGrid.SelectedObject = pair.Action;
        }

        private void btnOK_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}