using DesktopWidgets.Classes;

namespace DesktopWidgets.Events
{
    internal class HotkeyEvent : IEvent
    {
        public Hotkey Hotkey { get; set; } = new Hotkey();
    }
}