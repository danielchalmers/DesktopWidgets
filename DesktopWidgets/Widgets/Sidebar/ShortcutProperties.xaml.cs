#region

using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using Microsoft.Win32;

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
            cbWindowStyle.ItemsSource = Enum.GetValues(typeof (ProcessWindowStyle));

            if (shortcut == null)
                NewShortcut = new Shortcut();
            else
                NewShortcut = (Shortcut) shortcut.Clone();

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

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog();
            if (Directory.Exists(NewShortcut.Path) || File.Exists(NewShortcut.Path))
                dlg.InitialDirectory = Path.GetDirectoryName(NewShortcut.Path);
            if (dlg.ShowDialog() ?? false)
            {
                var path = dlg.FileName;

                NewShortcut.Path = path;
                txtPath.Text = path;

                if (string.IsNullOrWhiteSpace(txtName.Text) && File.Exists(path))
                {
                    var name = Path.GetFileNameWithoutExtension(path);
                    NewShortcut.Name = name;
                    txtName.Text = name;
                }
            }
        }
    }
}