using System.ComponentModel;
using System.Windows.Input;

namespace DesktopWidgets.Events
{
    public class WidgetMouseUpEvent : WidgetEventBase
    {
        [DisplayName("Mouse Button")]
        public MouseButton MouseButton { get; set; }
    }
}