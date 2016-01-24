using System;

namespace DesktopWidgets.WidgetBase.Interfaces
{
    internal interface IEventWidget
    {
        bool OpenOnEvent { get; set; }
        bool OpenOnEventStay { get; set; }
        TimeSpan OpenOnEventDuration { get; set; }
    }
}