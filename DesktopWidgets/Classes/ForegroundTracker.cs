using System;
using System.Linq;
using DesktopWidgets.Events;
using DesktopWidgets.Helpers;

namespace DesktopWidgets.Classes
{
    public static class ForegroundTracker
    {
        private static NativeMethods.WinEventDelegate _winEventDelegate;
        private static IntPtr m_hhook;

        private static bool _isForegroundFullscreen;
        private static string _foregroundTitle;

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

            foreach (var eventPair in App.WidgetsSettingsStore.EventActionPairs)
            {
                var evnt = eventPair.Event as ForegroundChangedEvent;
                if (evnt == null || eventPair.Disabled ||
                    !IsForegroundValid(foreground, evnt.FromMatchData, evnt.ToMatchData))
                {
                    continue;
                }
                eventPair.Action.Execute();
            }
            foreach (var widget in App.WidgetViews.Where(x => x.Settings.ForceTopmost))
            {
                widget.ThisApp?.BringToFront();
            }
        }

        private static bool IsForegroundValid(Win32App foreground, ForegroundMatchData fromData,
            ForegroundMatchData toData)
        {
            var oldTitle = _foregroundTitle;
            var oldFullscreen = _isForegroundFullscreen;
            _foregroundTitle = foreground.GetTitle();
            _isForegroundFullscreen = foreground.IsFullScreen();

            if (_foregroundTitle == null)
            {
                _foregroundTitle = string.Empty;
            }
            if (oldTitle == null)
            {
                oldTitle = string.Empty;
            }

            bool isTitleFromValid;
            bool isFullscreenFromValid;
            bool isTitleToValid;
            bool isFullscreenToValid;

            switch (fromData.TitleMatchMode)
            {
                case StringMatchMode.Any:
                    isTitleFromValid = true;
                    break;
                case StringMatchMode.Equals:
                    isTitleFromValid = oldTitle == fromData.Title;
                    break;
                case StringMatchMode.Contains:
                    isTitleFromValid = oldTitle.Contains(fromData.Title);
                    break;
                default:
                    isTitleFromValid = false;
                    break;
            }
            switch (toData.TitleMatchMode)
            {
                case StringMatchMode.Any:
                    isTitleToValid = true;
                    break;
                case StringMatchMode.Equals:
                    isTitleToValid = _foregroundTitle == toData.Title;
                    break;
                case StringMatchMode.Contains:
                    isTitleToValid = _foregroundTitle.Contains(toData.Title);
                    break;
                default:
                    isTitleToValid = false;
                    break;
            }
            switch (fromData.Fullscreen)
            {
                case YesNoAny.Any:
                    isFullscreenFromValid = true;
                    break;
                case YesNoAny.Yes:
                    isFullscreenFromValid = oldFullscreen;
                    break;
                case YesNoAny.No:
                    isFullscreenFromValid = !oldFullscreen;
                    break;
                default:
                    isFullscreenFromValid = false;
                    break;
            }
            switch (toData.Fullscreen)
            {
                case YesNoAny.Any:
                    isFullscreenToValid = true;
                    break;
                case YesNoAny.Yes:
                    isFullscreenToValid = _isForegroundFullscreen;
                    break;
                case YesNoAny.No:
                    isFullscreenToValid = !_isForegroundFullscreen;
                    break;
                default:
                    isFullscreenToValid = false;
                    break;
            }

            return isTitleFromValid && isFullscreenFromValid && isTitleToValid && isFullscreenToValid;
        }
    }
}