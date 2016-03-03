using System.ComponentModel;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace DesktopWidgets.Classes
{
    [ExpandableObject]
    public class ForegroundMatchData
    {
        [DisplayName("Title")]
        public string Title { get; set; }

        [DisplayName("Title Match Mode")]
        public StringMatchMode TitleMatchMode { get; set; } = StringMatchMode.Any;

        [DisplayName("Fullscreen")]
        public YesNoAny Fullscreen { get; set; } = YesNoAny.Any;
    }
}