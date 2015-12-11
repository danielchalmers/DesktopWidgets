using System.Collections.Generic;
using System.Windows.Controls;
using DesktopWidgets.Widgets.PictureFrame.OptionsPages;

namespace DesktopWidgets.Widgets.PictureFrame
{
    public static class Metadata
    {
        public const string FriendlyName = "Picture Frame";
        public static List<Page> OptionsPages = new List<Page> {new General(), new Style()};
    }
}