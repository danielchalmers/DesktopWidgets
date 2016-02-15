using System.Collections.Generic;
using System.ComponentModel;

namespace DesktopWidgets.Events
{
    internal class LaunchEvent : IEvent
    {
        [DisplayName("System Start")]
        public bool SystemStartup { get; set; } = false;

        [DisplayName("Parameters")]
        public List<string> Parameters { get; set; } = new List<string>();
    }
}