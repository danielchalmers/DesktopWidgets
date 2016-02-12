using System.ComponentModel;
using System.Windows.Input;

namespace DesktopWidgets.Events
{
    internal class WidgetMouseDoubleClickEvent : WidgetEventBase
    {
        [DisplayName("Mouse Button")]
        public MouseButton MouseButton { get; set; }
    }
}