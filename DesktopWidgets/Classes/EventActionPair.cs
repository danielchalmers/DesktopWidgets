using DesktopWidgets.Actions;
using DesktopWidgets.Events;

namespace DesktopWidgets.Classes
{
    public class EventActionPair
    {
        public EventActionId Identifier { get; set; } = new EventActionId();
        public IEvent Event { get; set; }
        public ActionBase Action { get; set; }
        public bool Disabled { get; set; } = false;
        public string Name { get; set; } = "Untitled";
    }
}