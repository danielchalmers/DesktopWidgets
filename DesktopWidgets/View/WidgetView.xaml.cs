using System;
using System.Windows;

namespace DesktopWidgets.View
{
    /// <summary>
    ///     Interaction logic for View.xaml
    /// </summary>
    public partial class WidgetView : Window
    {
        public Guid Guid;

        public WidgetView(Guid guid)
        {
            InitializeComponent();
            Guid = guid;
        }
    }
}