#region

using System;
using System.Diagnostics;

#endregion

namespace DesktopWidgets.Widgets.Sidebar
{
    public class Shortcut : ICloneable
    {
        public string Name { get; set; } = "";
        public string Path { get; set; } = "";
        public string Args { get; set; } = "";
        public string SpecialType { get; set; } = "";
        public string StartInFolder { get; set; } = "";
        public ProcessWindowStyle WindowStyle { get; set; } = ProcessWindowStyle.Normal;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}