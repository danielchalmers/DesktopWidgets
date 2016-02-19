using System.ComponentModel;

namespace DesktopWidgets.Events
{
    internal class ForegroundTitleChangedEvent : IEvent
    {
        [DisplayName("From Title")]
        public string FromTitle { get; set; }

        [DisplayName("From Title Match Mode")]
        public MatchMode FromTitleMatchMode { get; set; }

        [DisplayName("To Title")]
        public string ToTitle { get; set; }

        [DisplayName("To Title Match Mode")]
        public MatchMode ToTitleMatchMode { get; set; }
    }
}