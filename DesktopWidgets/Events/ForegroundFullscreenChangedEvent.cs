using System.ComponentModel;

namespace DesktopWidgets.Events
{
    internal class ForegroundFullscreenChangedEvent : IEvent
    {
        [DisplayName("From Fullscreen")]
        public YesNoAny FromFullscreen { get; set; } = YesNoAny.Any;

        [DisplayName("To Fullscreen")]
        public YesNoAny ToFullscreen { get; set; } = YesNoAny.Any;
    }
}