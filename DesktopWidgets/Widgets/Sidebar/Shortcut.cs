#region

using System;
using System.ComponentModel;
using DesktopWidgets.Classes;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

#endregion

namespace DesktopWidgets.Widgets.Sidebar
{
    [DisplayName("Shortcut")]
    [ExpandableObject]
    public class Shortcut : ICloneable
    {
        [Browsable(false)]
        [DisplayName("Guid")]
        public Guid Guid { get; set; } = Guid.NewGuid();

        [DisplayName("Name")]
        public string Name { get; set; }

        [Browsable(false)]
        [DisplayName("Special Type")]
        public string SpecialType { get; set; }

        [DisplayName("Icon Path")]
        public string IconPath { get; set; }

        [DisplayName("Process File")]
        public ProcessFile ProcessFile { get; set; } = new ProcessFile();

        [DisplayName("Hotkey")]
        public Hotkey Hotkey { get; set; } = new Hotkey();

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}