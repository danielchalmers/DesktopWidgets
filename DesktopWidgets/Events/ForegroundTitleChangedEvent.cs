using System.ComponentModel;

namespace DesktopWidgets.Events
{
    internal class ForegroundTitleChangedEvent : IEvent
    {
        [DisplayName("From Title")]
        public string FromTitle { get; set; }

        [DisplayName("To Title")]
        public string ToTitle { get; set; }
    }
}