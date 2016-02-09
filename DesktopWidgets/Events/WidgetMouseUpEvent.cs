using System.Windows.Input;

namespace DesktopWidgets.Events
{
    internal class WidgetMouseUpEvent : WidgetEventBase
    {
        public MouseButton MouseButton { get; set; }
    }
}