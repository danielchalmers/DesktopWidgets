using System.ComponentModel;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace DesktopWidgets.Classes
{
    [ExpandableObject]
    [DisplayName("Speech Settings")]
    public class SpeechSettings
    {
        public int Rate { get; set; } = 0;
        public int Volume { get; set; } = 100;
    }
}