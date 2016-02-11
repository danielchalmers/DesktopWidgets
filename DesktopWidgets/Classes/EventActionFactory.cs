using System.Collections.Generic;
using DesktopWidgets.Actions;
using DesktopWidgets.Events;

namespace DesktopWidgets.Classes
{
    public static class EventActionFactory
    {
        public static readonly List<string> AvailableEvents = new List<string>
        {
            "Widget Mouse Down",
            "Widget Mouse Up",
            "Widget Enable",
            "Widget Disable",
            "Widget Intro",
            "Widget Dismiss"
        };

        public static readonly List<string> AvailableActions = new List<string>
        {
            "Open File",
            "Play Sound",
            "Widget Enable",
            "Widget Disable",
            "Widget Intro",
            "Widget Dismiss",
            "Widget Hide"
        };

        public static IEvent GetNewEventFromName(string name)
        {
            switch (name)
            {
                case "Widget Mouse Down":
                    return new WidgetMouseDownEvent();
                case "Widget Mouse Up":
                    return new WidgetMouseUpEvent();
                case "Widget Enable":
                    return new WidgetEnableEvent();
                case "Widget Disable":
                    return new WidgetDisableEvent();
                case "Widget Intro":
                    return new WidgetIntroEvent();
                case "Widget Dismiss":
                    return new WidgetDismissEvent();
            }
            return null;
        }

        public static IAction GetNewActionFromName(string name)
        {
            switch (name)
            {
                case "Open File":
                    return new OpenFileAction();
                case "Play Sound":
                    return new PlaySoundAction();
                case "Widget Enable":
                    return new WidgetEnableAction();
                case "Widget Disable":
                    return new WidgetDisableAction();
                case "Widget Intro":
                    return new WidgetIntroAction();
                case "Widget Dismiss":
                    return new WidgetDismissAction();
                case "Widget Hide":
                    return new WidgetHideAction();
            }
            return null;
        }

        public static string GetNameFromEvent(IEvent evnt)
        {
            if (evnt is WidgetMouseDownEvent)
                return "Widget Mouse Down";
            if (evnt is WidgetMouseUpEvent)
                return "Widget Mouse Up";
            if (evnt is WidgetEnableEvent)
                return "Widget Enable";
            if (evnt is WidgetDisableEvent)
                return "Widget Disable";
            if (evnt is WidgetIntroEvent)
                return "Widget Intro";
            if (evnt is WidgetDismissEvent)
                return "Widget Dismiss";
            return null;
        }

        public static string GetNameFromAction(IAction action)
        {
            if (action is OpenFileAction)
                return "Open File";
            if (action is PlaySoundAction)
                return "Play Sound";
            if (action is WidgetEnableAction)
                return "Widget Enable";
            if (action is WidgetDisableAction)
                return "Widget Disable";
            if (action is WidgetIntroAction)
                return "Widget Intro";
            if (action is WidgetDismissAction)
                return "Widget Dismiss";
            if (action is WidgetHideAction)
                return "Widget Hide";
            return null;
        }
    }
}