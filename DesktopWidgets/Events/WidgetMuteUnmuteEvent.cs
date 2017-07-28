using System.ComponentModel;

namespace DesktopWidgets.Events
{
    public class WidgetMuteUnmuteEvent : IEvent
    {
        [DisplayName("Mode")]
        public MuteEventMode Mode { get; set; } = MuteEventMode.Both;
    }
}