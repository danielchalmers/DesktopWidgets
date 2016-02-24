#region

using System.Threading;
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

        public static void ShowAsync(string text, MessageBoxButton button = MessageBoxButton.OK,
            MessageBoxImage image = MessageBoxImage.Information, MessageBoxResult defaultButton = MessageBoxResult.OK)
        {
            new Thread(
                new ThreadStart(delegate { MessageBox.Show(text, Resources.AppName, button, image, defaultButton); }))
                .Start();
        }
    }
}