using System.Windows.Input;

namespace DesktopWidgets.Events
{
    internal class WidgetMouseDownEvent : WidgetEventBase
    {
        public MouseButton MouseButton { get; set; }
    }
}