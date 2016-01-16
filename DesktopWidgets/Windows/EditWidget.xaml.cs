using System.Windows;
using DesktopWidgets.Classes;
using DesktopWidgets.Helpers;

namespace DesktopWidgets.Windows
{
    /// <summary>
    ///     Interaction logic for EditWidget.xaml
    /// </summary>
    public partial class EditWidget : Window
    {
        private readonly HorizontalAlignment _previousHorizontalAlignment;
        private readonly bool _previousIsDocked;
        private readonly VerticalAlignment _previousVerticalAlignment;
        private readonly WidgetId Id;

        public EditWidget(WidgetId id)
        {
            InitializeComponent();
            Id = id;
            var settings = id.GetSettings();

            Title = $"Edit {id.GetName()}";

            _previousHorizontalAlignment = settings.HorizontalAlignment;
            _previousVerticalAlignment = settings.VerticalAlignment;
            _previousIsDocked = settings.IsDocked;

            PropertyGrid.SelectedObject = settings;
        }

        private void btnOK_OnClick(object sender, RoutedEventArgs e)
        {
            Hide();
            Id.GetView()?
                .UpdateUi(isDocked: _previousIsDocked, dockHorizontalAlignment: _previousHorizontalAlignment,
                    dockVerticalAlignment: _previousVerticalAlignment);
        }
    }
}