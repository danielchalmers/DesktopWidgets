using System.ComponentModel;

namespace DesktopWidgets.Events
{
    public class MuteUnmuteEvent : IEvent
    {
        [DisplayName("Mode")]
        public MuteEventMode Mode { get; set; } = MuteEventMode.Both;
    }
}