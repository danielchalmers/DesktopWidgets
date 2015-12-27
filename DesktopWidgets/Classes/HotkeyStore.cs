using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using DesktopWidgets.Helpers;
using NHotkey;
using NHotkey.Wpf;

namespace DesktopWidgets.Classes
{
    internal class HotkeyStore
    {
        private static readonly Dictionary<Hotkey, Action> Hotkeys = new Dictionary<Hotkey, Action>();

        public static void RegisterHotkey(Hotkey hotkey, Action callback)
        {
            if (Hotkeys.ContainsKey(hotkey))
                Hotkeys[hotkey] = callback;
            else
                Hotkeys.Add(hotkey, callback);

            try
            {
                HotkeyManager.Current.AddOrReplace($"{hotkey.Key}\\{hotkey.ModifierKeys}", hotkey.Key,
                    hotkey.ModifierKeys, OnHotkey);
            }
            catch (HotkeyAlreadyRegisteredException)
            {
            }
        }

        private static void OnHotkey(object sender, HotkeyEventArgs e)
        {
            var keys = e.Name.Split('\\');
            if (keys.Length != 2)
                return;
            Key key;
            Enum.TryParse(keys[0], out key);
            ModifierKeys modifierKeys;
            Enum.TryParse(keys[1], out modifierKeys);
            foreach (var hotkey in Hotkeys.Where(x => x.Key.Key == key && x.Key.ModifierKeys == modifierKeys))
            {
                if (!hotkey.Key.WorksIfForegroundIsFullscreen && FullScreenHelper.DoesAnyMonitorHaveFullscreenApp())
                    continue;
                hotkey.Value?.Invoke();
            }
        }
    }
}