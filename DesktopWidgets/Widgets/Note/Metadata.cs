using System.Collections.Generic;
using System.Windows.Controls;
using DesktopWidgets.Widgets.Note.OptionsPages;

namespace DesktopWidgets.Widgets.Note
{
    public static class Metadata
    {
        public const string FriendlyName = "Note";
        public static List<Page> OptionsPages = new List<Page> {new General(), new Style()};
    }
}