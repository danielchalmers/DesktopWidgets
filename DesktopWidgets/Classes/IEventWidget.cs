using System;

namespace DesktopWidgets.Classes
{
    internal interface IEventWidget
    {
        bool OpenOnEvent { get; set; }
        bool OpenOnEventStay { get; set; }
        TimeSpan OpenOnEventDuration { get; set; }
    }
}