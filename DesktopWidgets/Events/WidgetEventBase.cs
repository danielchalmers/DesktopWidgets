using System.ComponentModel;
using DesktopWidgets.WidgetBase;

namespace DesktopWidgets.Events
{
    public abstract class WidgetEventBase : IEvent
    {
        [DisplayName("Widget")]
        public WidgetId WidgetId { get; set; }
    }
}