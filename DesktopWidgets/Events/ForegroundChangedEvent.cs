using System.ComponentModel;

namespace DesktopWidgets.Events
{
    internal class ForegroundChangedEvent : IEvent
    {
        [DisplayName("From Title")]
        public string FromTitle { get; set; }

        [DisplayName("From Title Match Mode")]
        public MatchMode FromTitleMatchMode { get; set; }

        [DisplayName("From Fullscreen")]
        public YesNoAny FromFullscreen { get; set; } = YesNoAny.Any;

        [DisplayName("To Title")]
        public string ToTitle { get; set; }

        [DisplayName("To Title Match Mode")]
        public MatchMode ToTitleMatchMode { get; set; }

        [DisplayName("To Fullscreen")]
        public YesNoAny ToFullscreen { get; set; } = YesNoAny.Any;
    }
}