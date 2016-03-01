using System.ComponentModel;
using DesktopWidgets.Classes;

namespace DesktopWidgets.Events
{
    internal class ForegroundChangedEvent : IEvent
    {
        [DisplayName("From")]
        public ForegroundMatchData FromMatchData { get; set; } = new ForegroundMatchData();

        [DisplayName("To")]
        public ForegroundMatchData ToMatchData { get; set; } = new ForegroundMatchData();
    }
}