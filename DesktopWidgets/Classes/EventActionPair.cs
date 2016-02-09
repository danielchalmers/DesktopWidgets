using DesktopWidgets.Actions;
using DesktopWidgets.Events;

namespace DesktopWidgets.Classes
{
    public class EventActionPair
    {
        public IEvent Event { get; set; }
        public IAction Action { get; set; }
    }
}