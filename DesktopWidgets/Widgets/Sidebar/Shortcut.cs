#region

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;

#endregion

namespace DesktopWidgets.Widgets.Sidebar
{
    public class Shortcut : ICloneable
    {
        public Guid Guid { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = "";
        public string Path { get; set; } = "";
        public string Args { get; set; } = "";
        public string SpecialType { get; set; } = "";
        public string StartInFolder { get; set; } = "";
        public ProcessWindowStyle WindowStyle { get; set; } = ProcessWindowStyle.Normal;
        public Key HotKey { get; set; } = Key.None;
        public ModifierKeys HotKeyModifiers { get; set; } = ModifierKeys.None;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}