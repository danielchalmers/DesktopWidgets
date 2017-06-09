#region

using System.Windows;

#endregion

namespace DesktopWidgets.Widgets.Sidebar
{
    /// <summary>
    ///     Interaction logic for ShortcutProperties.xaml
    /// </summary>
    public partial class ShortcutProperties : Window
    {
        public Shortcut NewShortcut;

        public ShortcutProperties(Shortcut shortcut = null)
        {
            InitializeComponent();
            Title = shortcut == null ? "New Shortcut" : "Edit Shortcut";

            if (shortcut == null)
            {
                NewShortcut = new Shortcut();
            }
            else
            {
                NewShortcut = (Shortcut)shortcut.Clone();
            }

            DataContext = NewShortcut;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            NewShortcut = null;
            DialogResult = true;
        }
    }
}