using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using DesktopWidgets.Helpers;
using NHotkey;
using NHotkey.Wpf;

namespace DesktopWidgets.Classes
{
    internal static class HotkeyStore
    {
        private static readonly Dictionary<Guid, Tuple<Hotkey, Action>> Hotkeys =
            new Dictionary<Guid, Tuple<Hotkey, Action>>();

        public static void RegisterHotkey(Guid guid, Hotkey hotkey, Action callback)
        {
            var dictionaryHotkey = new Tuple<Hotkey, Action>(hotkey, callback);
            if (Hotkeys.ContainsKey(guid))
            {
                Hotkeys[guid] = dictionaryHotkey;
                UnregisterHotkey(guid);
            }
            else
            {
                Hotkeys.Add(guid, dictionaryHotkey);
            }

            try
            {
                HotkeyManager.Current.AddOrReplace($"{hotkey.Key}\\{hotkey.ModifierKeys}", hotkey.Key,
                    hotkey.ModifierKeys, OnHotkey);
            }
            catch (HotkeyAlreadyRegisteredException)
            {
            }
        }

        public static void RemoveHotkey(Guid guid)
        {
            Hotkeys.Remove(guid);
        }

        private static void UnregisterHotkey(Guid guid)
        {
            if (!Hotkeys.ContainsKey(guid))
                return;
            if (
                Hotkeys.Count(
                    x =>
                        x.Value.Item1.Key == Hotkeys[guid].Item1.Key &&
                        x.Value.Item1.ModifierKeys == Hotkeys[guid].Item1.ModifierKeys) == 0)
                UnregisterHotkey(Hotkeys[guid].Item1);
        }

        private static void UnregisterHotkey(Hotkey hotkey)
        {
            try
            {
                HotkeyManager.Current.Remove($"{hotkey.Key}\\{hotkey.ModifierKeys}");
            }
            catch
            {
                // ignored
            }
        }

        private static void OnHotkey(object sender, HotkeyEventArgs e)
        {
            if (App.IsMuted)
                return;
            var keys = e.Name.Split('\\');
            if (keys.Length != 2)
                return;
            Key key;
            Enum.TryParse(keys[0], out key);
            ModifierKeys modifierKeys;
            Enum.TryParse(keys[1], out modifierKeys);
            foreach (
                var hotkey in Hotkeys.Where(x => x.Value.Item1.Key == key && x.Value.Item1.ModifierKeys == modifierKeys)
                )
            {
                if (!hotkey.Value.Item1.WorksIfForegroundIsFullscreen &&
                    FullScreenHelper.DoesAnyMonitorHaveFullscreenApp())
                    continue;
                hotkey.Value.Item2?.Invoke();
            }
        }
    }
}