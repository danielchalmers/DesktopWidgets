using System;
using System.ComponentModel;
using DesktopWidgets.Classes;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace DesktopWidgets.WidgetBase
{
    [ExpandableObject]
    [DisplayName("Widget Package Info")]
    public class WidgetPackageInfo
    {
        [PropertyOrder(1)]
        [DisplayName("Publisher")]
        public string Publisher { get; set; } = "";

        [PropertyOrder(0)]
        [DisplayName("Name")]
        public string Name { get; set; } = "";

        [Browsable(false)]
        [DisplayName("Publish Date/Time")]
        public DateTime PublishDateTime { get; set; } = DateTime.Now;

        [Browsable(false)]
        [DisplayName("App Version")]
        public Version AppVersion { get; set; } = AssemblyInfo.Version;

        public override string ToString()
        {
            return $"\"{Name}\" by {Publisher}";
        }
    }
}