using System.ComponentModel;
using DesktopWidgets.WidgetBase;

namespace DesktopWidgets.Events
{
    internal class WidgetEventBase : IEvent
    {
        [DisplayName("Widget")]
        public WidgetId WidgetId { get; set; }
    }
}