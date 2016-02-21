using System.ComponentModel;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace DesktopWidgets.Classes
{
    [ExpandableObject]
    [DisplayName("Intro Settings")]
    public class IntroData
    {
        [DisplayName("Duration")]
        public int Duration { get; set; } = -1;

        [DisplayName("Reversable")]
        public bool Reversable { get; set; } = false;

        [DisplayName("Activate")]
        public bool Activate { get; set; } = false;

        [DisplayName("Hide On Finish")]
        public bool HideOnFinish { get; set; } = true;

        [DisplayName("Execute Finish Action")]
        public bool ExecuteFinishAction { get; set; } = true;
    }
}