using System.ComponentModel;

namespace DesktopWidgets.Events
{
    public class TrayIconClickEvent : IEvent
    {
        [DisplayName("Double Click")]
        public bool DoubleClick { get; set; } = false;
    }
}