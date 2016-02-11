using System.ComponentModel;
using System.Windows.Input;

namespace DesktopWidgets.Events
{
    internal class WidgetMouseUpEvent : WidgetEventBase
    {
        [DisplayName("Mouse Button")]
        public MouseButton MouseButton { get; set; }
    }
}