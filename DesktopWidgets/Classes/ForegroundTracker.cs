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
                if (((evnt.FromMatchData.Fullscreen == YesNoAny.Any) ||
                     (evnt.FromMatchData.Fullscreen == YesNoAny.Yes && oldFullscreen) ||
                     (evnt.FromMatchData.Fullscreen == YesNoAny.No && !oldFullscreen)) &&
                    ((evnt.ToMatchData.Fullscreen == YesNoAny.Any) ||
                     (evnt.ToMatchData.Fullscreen == YesNoAny.Yes && IsForegroundFullscreen) ||
                     (evnt.ToMatchData.Fullscreen == YesNoAny.No && !IsForegroundFullscreen)) &&
                    (evnt.ToMatchData.TitleMatchMode == StringMatchMode.Any ||
                     (evnt.ToMatchData.TitleMatchMode == StringMatchMode.Equals &&
                      ForegroundTitle == evnt.ToMatchData.Title) ||
                     (evnt.ToMatchData.TitleMatchMode == StringMatchMode.Contains &&
                      ForegroundTitle.Contains(evnt.ToMatchData.Title))) &&
                    (evnt.ToMatchData.TitleMatchMode == StringMatchMode.Any ||
                     (evnt.FromMatchData.TitleMatchMode == StringMatchMode.Equals &&
                      oldTitle == evnt.FromMatchData.Title) ||
                     (evnt.FromMatchData.TitleMatchMode == StringMatchMode.Contains &&
                      oldTitle.Contains(evnt.FromMatchData.Title))))
                    eventPair.Action.Execute();
            }
            foreach (var widget in App.WidgetViews.Where(x => x.Settings.ForceTopmost))
                widget.ThisApp?.BringToFront();
        }
    }
}