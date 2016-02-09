using DesktopWidgets.WidgetBase;

namespace DesktopWidgets.Events
{
    internal class WidgetEventBase : IEvent
    {
        public WidgetId WidgetId { get; set; }
    }
}