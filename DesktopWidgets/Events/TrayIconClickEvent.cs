using System.ComponentModel;

namespace DesktopWidgets.Events
{
    internal class TrayIconClickEvent : IEvent
    {
        [DisplayName("Double Click")]
        public bool DoubleClick { get; set; } = false;
    }
}