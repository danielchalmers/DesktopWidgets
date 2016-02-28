using System.ComponentModel;

namespace DesktopWidgets.Events
{
    internal class MuteUnmuteEvent : IEvent
    {
        [DisplayName("Mode")]
        public MuteEventMode Mode { get; set; } = MuteEventMode.All;
    }
}