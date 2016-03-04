using System.ComponentModel;

namespace DesktopWidgets.Events
{
    internal class WidgetMuteUnmuteEvent : IEvent
    {
        [DisplayName("Mode")]
        public MuteEventMode Mode { get; set; } = MuteEventMode.Both;
    }
}