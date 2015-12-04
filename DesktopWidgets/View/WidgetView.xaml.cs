using System.Windows;
using DesktopWidgets.Classes;
using DesktopWidgets.ViewModelBase;

namespace DesktopWidgets.View
{
    /// <summary>
    ///     Interaction logic for View.xaml
    /// </summary>
    public partial class WidgetView : Window
    {
        public WidgetView(WidgetId id)
        {
            InitializeComponent();
            Id = id;
        }

        public WidgetId Id { get; private set; }
        public bool AnimationRunning { get; set; } = false;

        public void UpdateUi()
        {
            ((WidgetViewModelBase) DataContext).UpdateUi();
        }
    }
}