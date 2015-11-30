using System.Windows;
using DesktopWidgets.Classes;

namespace DesktopWidgets.View
{
    /// <summary>
    ///     Interaction logic for View.xaml
    /// </summary>
    public partial class WidgetView : Window
    {
        public WidgetId ID;

        public WidgetView(WidgetId id)
        {
            InitializeComponent();
            ID = id;
        }
    }
}