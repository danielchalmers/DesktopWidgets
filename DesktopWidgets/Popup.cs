#region

using System.Windows;
using DesktopWidgets.Properties;

#endregion

namespace DesktopWidgets
{
    internal class Popup
    {
        public static MessageBoxResult Show(string text, MessageBoxButton button = MessageBoxButton.OK,
            MessageBoxImage image = MessageBoxImage.Information, MessageBoxResult defaultButton = MessageBoxResult.OK)
        {
            try
            {
                return MessageBox.Show(text, Resources.AppName, button, image, defaultButton);
            }
            catch
            {
                return MessageBoxResult.Cancel;
            }
        }
    }
}