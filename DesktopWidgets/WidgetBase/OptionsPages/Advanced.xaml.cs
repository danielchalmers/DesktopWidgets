using System.Windows.Controls;

namespace DesktopWidgets.WidgetBase.OptionsPages
{
    /// <summary>
    ///     Interaction logic for Advanced.xaml
    /// </summary>
    public partial class Advanced : Page
    {
        public Advanced(object selectedObject)
        {
            InitializeComponent();
            PropertyGrid.SelectedObject = selectedObject;
        }
    }
}