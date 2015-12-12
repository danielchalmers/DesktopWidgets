using System.Windows.Controls;

namespace DesktopWidgets.OptionsPages
{
    /// <summary>
    ///     Interaction logic for PropertyView.xaml
    /// </summary>
    public partial class PropertyView : Page
    {
        public PropertyView(object selectedObject, string title)
        {
            InitializeComponent();
            Title = title;
            PropertyGrid.SelectedObject = selectedObject;
        }
    }
}