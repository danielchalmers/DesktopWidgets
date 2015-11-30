#region

using System.Windows;
using DesktopWidgets.Properties;

#endregion

namespace DesktopWidgets.Classes
{
    internal static class Popup
    {
        public static MessageBoxResult Show(string text, MessageBoxButton button = MessageBoxButton.OK,
            MessageBoxImage image = MessageBoxImage.Information, MessageBoxResult defaultButton = MessageBoxResult.OK)
        {
            try
            {
                App.HelperWindow.Show();
                var msg = MessageBox.Show(App.HelperWindow, text, Resources.AppName, button, image, defaultButton);
                App.HelperWindow.Hide();
                return msg;
            }
            catch
            {
                return MessageBoxResult.Cancel;
            }
        }
    }
}