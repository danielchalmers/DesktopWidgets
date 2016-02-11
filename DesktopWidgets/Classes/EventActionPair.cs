﻿using DesktopWidgets.Actions;
using DesktopWidgets.Events;

namespace DesktopWidgets.Classes
{
    public class EventActionPair
    {
        public EventActionId Identifier { get; set; } = new EventActionId();
        public IEvent Event { get; set; }
        public IAction Action { get; set; }
    }
}