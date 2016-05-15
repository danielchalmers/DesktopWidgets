using System.ComponentModel;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace DesktopWidgets.Classes
{
    [ExpandableObject]
    [DisplayName("Intro Settings")]
    public class IntroData
    {
        [PropertyOrder(0)]
        [DisplayName("Duration")]
        public int Duration { get; set; } = -1;

        [PropertyOrder(2)]
        [DisplayName("Reversable")]
        public bool Reversable { get; set; } = false;

        [PropertyOrder(1)]
        [DisplayName("Activate")]
        public bool Activate { get; set; } = false;

        [PropertyOrder(3)]
        [DisplayName("Hide On End")]
        public bool HideOnFinish { get; set; } = true;

        [PropertyOrder(4)]
        [DisplayName("Trigger End Event")]
        public bool ExecuteFinishAction { get; set; } = true;
    }
}