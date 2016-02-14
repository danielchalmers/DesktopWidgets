#region

using System;
using DesktopWidgets.Classes;

#endregion

namespace DesktopWidgets.Widgets.Sidebar
{
    public class Shortcut : ICloneable
    {
        public Guid Guid { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string SpecialType { get; set; }
        public string IconPath { get; set; }
        public ProcessFile ProcessFile { get; set; } = new ProcessFile();
        public Hotkey Hotkey { get; set; } = new Hotkey();

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}