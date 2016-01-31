using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using DesktopWidgets.Classes;
using DesktopWidgets.Helpers;
using NHotkey;
using NHotkey.Wpf;

namespace DesktopWidgets.Stores
{
    internal static class HotkeyStore
    {
        private static readonly List<Tuple<Hotkey, Action>> Hotkeys = new List<Tuple<Hotkey, Action>>();

        public static void RegisterHotkey(Hotkey hotkey, Action callback)
        {
            foreach (var hk in Hotkeys.Where(x => x.Item1.Guid == hotkey.Guid).ToList())
            {
                Hotkeys.Remove(hk);
                if (Hotkeys.All(x => x.Item1.Key != hotkey.Key && x.Item1.ModifierKeys != hotkey.ModifierKeys))
                    UnregisterHotkey(hk.Item1);
            }
            Hotkeys.Add(new Tuple<Hotkey, Action>(hotkey, callback));

            try
            {
                HotkeyManager.Current.AddOrReplace(hotkey.Guid.ToString(),
                    hotkey.Key, hotkey.ModifierKeys,
                    (sender, args) => OnHotkey(hotkey.Key, hotkey.ModifierKeys));
            }
            catch (HotkeyAlreadyRegisteredException)
            {
            }
        }

        public static void RemoveHotkey(Guid guid)
        {
            foreach (var hk in Hotkeys.Where(x => x.Item1.Guid == guid))
                Hotkeys.Remove(hk);
        }

        private static void UnregisterHotkey(Hotkey hotkey)
        {
            try
            {
                HotkeyManager.Current.Remove(hotkey.Guid.ToString());
            }
            catch
            {
                // ignored
            }
        }

        private static void OnHotkey(Key key, ModifierKeys modifierKeys)
        {
            foreach (
                var hotkey in
                    Hotkeys.Where(x => x.Item1.Key == key && x.Item1.ModifierKeys == modifierKeys)
                        .Where(
                            hotkey =>
                                (hotkey.Item1.WorksIfForegroundIsFullscreen ||
                                 !FullScreenHelper.DoesAnyMonitorHaveFullscreenApp()) &&
                                (!hotkey.Item1.WorksIfMuted || !App.IsMuted)))
            {
                hotkey.Item2?.Invoke();
            }
        }
    }
}