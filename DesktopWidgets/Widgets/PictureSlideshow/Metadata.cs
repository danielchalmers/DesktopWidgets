using System.Collections.Generic;
using System.Windows.Controls;
using DesktopWidgets.Widgets.PictureSlideshow.OptionsPages;

namespace DesktopWidgets.Widgets.PictureSlideshow
{
    public static class Metadata
    {
        public const string FriendlyName = "Picture Slideshow";
        public static List<Page> OptionsPages = new List<Page> {new General(), new Style()};
    }
}