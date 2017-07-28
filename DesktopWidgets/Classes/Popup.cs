using System.Threading;
using System.Windows;
using DesktopWidgets.Properties;

namespace DesktopWidgets.Classes
{
    public static class Popup
    {
        public static MessageBoxResult Show(string text, MessageBoxButton button = MessageBoxButton.OK,
            MessageBoxImage image = MessageBoxImage.Information, MessageBoxResult defaultButton = MessageBoxResult.OK)
        {
            return MessageBox.Show(text, Resources.AppName, button, image, defaultButton);
        }

        public static void ShowAsync(string text, MessageBoxButton button = MessageBoxButton.OK,
            MessageBoxImage image = MessageBoxImage.Information, MessageBoxResult defaultButton = MessageBoxResult.OK)
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                MessageBox.Show(text, Resources.AppName, button, image, defaultButton);
            }).Start();
        }
    }
}