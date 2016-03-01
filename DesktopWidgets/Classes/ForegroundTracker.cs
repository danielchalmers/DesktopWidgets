using System;
using System.Linq;
using DesktopWidgets.Events;
using DesktopWidgets.Helpers;

namespace DesktopWidgets.Classes
{
    internal static class ForegroundTracker
    {
        private static NativeMethods.WinEventDelegate _winEventDelegate;
        private static IntPtr m_hhook;

        public static bool IsForegroundFullscreen;
        public static string ForegroundTitle;

        public static void AddHook()
        {
            _winEventDelegate = WinEventProc;
            m_hhook = NativeMethods.SetWinEventHook(NativeMethods.EVENT_SYSTEM_FOREGROUND,
                NativeMethods.EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, _winEventDelegate, 0, 0,
                NativeMethods.WINEVENT_OUTOFCONTEXT);
        }

        private static void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild,
            uint dwEventThread, uint dwmsEventTime)
        {
            var foreground = Win32Helper.GetForegroundApp();
            var oldTitle = ForegroundTitle;
            var oldFullscreen = IsForegroundFullscreen;
            ForegroundTitle = foreground.GetTitle();
            IsForegroundFullscreen = foreground.IsFullScreen();

            if (ForegroundTitle == null)
                ForegroundTitle = string.Empty;
            if (oldTitle == null)
                oldTitle = string.Empty;

            foreach (var eventPair in App.WidgetsSettingsStore.EventActionPairs)
            {
                var evnt = eventPair.Event as ForegroundChangedEvent;
                if (evnt == null)
                    continue;
                if (((evnt.FromFullscreen == YesNoAny.Any) ||
                     (evnt.FromFullscreen == YesNoAny.Yes && oldFullscreen) ||
                     (evnt.FromFullscreen == YesNoAny.No && !oldFullscreen)) &&
                    ((evnt.ToFullscreen == YesNoAny.Any) ||
                     (evnt.ToFullscreen == YesNoAny.Yes && IsForegroundFullscreen) ||
                     (evnt.ToFullscreen == YesNoAny.No && !IsForegroundFullscreen)) &&
                    (string.IsNullOrWhiteSpace(evnt.ToTitle) ||
                     (evnt.ToTitleMatchMode == MatchMode.Equals && ForegroundTitle == evnt.ToTitle) ||
                     (evnt.ToTitleMatchMode == MatchMode.Contains && ForegroundTitle.Contains(evnt.ToTitle))) &&
                    (string.IsNullOrWhiteSpace(evnt.FromTitle) ||
                     (evnt.FromTitleMatchMode == MatchMode.Equals && oldTitle == evnt.FromTitle) ||
                     (evnt.FromTitleMatchMode == MatchMode.Contains && oldTitle.Contains(evnt.FromTitle))))
                    eventPair.Action.Execute();
            }
            foreach (var widget in App.WidgetViews.Where(x => x.Settings.OnTop))
                widget.ThisApp?.BringToFront();
        }
    }
}