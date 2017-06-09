using System;
using System.ComponentModel;
using DesktopWidgets.Classes;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace DesktopWidgets.Widgets.Sidebar
{
    [DisplayName("Shortcut")]
    [ExpandableObject]
    public class Shortcut : ICloneable
    {
        [Browsable(false)]
        [DisplayName("Guid")]
        public Guid Guid { get; set; } = Guid.NewGuid();

        [PropertyOrder(0)]
        [DisplayName("Name")]
        public string Name { get; set; }

        [Browsable(false)]
        [DisplayName("Special Type")]
        public string SpecialType { get; set; }

        [PropertyOrder(3)]
        [DisplayName("Icon Path")]
        public string IconPath { get; set; }

        [PropertyOrder(1)]
        [DisplayName("Process File")]
        public ProcessFile ProcessFile { get; set; } = new ProcessFile();

        [PropertyOrder(2)]
        [DisplayName("Hotkey")]
        public Hotkey Hotkey { get; set; } = new Hotkey();

        public object Clone()
        {
            return MemberwiseClone();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}