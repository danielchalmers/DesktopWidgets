using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DesktopWidgets.Classes;
using DesktopWidgets.Helpers;
using DesktopWidgets.Properties;
using DesktopWidgets.Stores;

namespace DesktopWidgets.Widgets.Sidebar
{
    public static class ShortcutHelper
    {
        public static string GetName(Shortcut shortcut)
            => string.IsNullOrWhiteSpace(shortcut.Name) ? shortcut.ProcessFile.Path : shortcut.Name;

        private static string GetNameFromPath(string path)
        {
            var root = Path.GetPathRoot(path);
            if (path == root)
            {
                return root;
            }
            if (File.Exists(path))
            {
                return Path.GetFileNameWithoutExtension(path);
            }
            return path;
        }

        public static void ProcessFile(this ViewModel viewModel, string filepath, string name = "", bool msg = true)
        {
            if (viewModel.Settings.ParseShortcutFiles && Path.GetExtension(filepath) == ".lnk")
            {
                filepath = FileSystemHelper.GetShortcutTargetFile(filepath);
            }
            viewModel.Add(new Shortcut
            {
                Name = name == "" ? GetNameFromPath(filepath) : name,
                ProcessFile = new ProcessFile(filepath)
            }, msg);
        }

        public static void ProcessFiles(this ViewModel viewModel, string[] files, bool msg = true)
        {
            if (files.Length >= 5 && msg &&
                Popup.Show($"You are attempting to add {files.Length} shortcuts.\n\nAre you sure?",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes) == MessageBoxResult.No)
            {
                return;
            }
            foreach (var file in files)
            {
                viewModel.ProcessFile(file);
            }
        }

        public static void Add(this ViewModel viewModel, Shortcut shortcut, bool msg = true)
        {
            if (msg && viewModel.Settings.Shortcuts.Any(x => x.ProcessFile.Path == shortcut.ProcessFile.Path))
            {
                if (
                    Popup.Show(
                        "A shortcut with this path already exists.\n\nAre you sure you want to add this shortcut?",
                        MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes) == MessageBoxResult.No)
                {
                    return;
                }
            }
            viewModel.Settings.Shortcuts.Add(shortcut);
        }

        public static void OpenFolder(this Shortcut shortcut)
        {
            ProcessHelper.OpenFolder(shortcut.ProcessFile.Path);
        }

        public static void New(this ViewModel viewModel)
        {
            var dialog = new ShortcutProperties();
            dialog.ShowDialog();
            if (string.IsNullOrWhiteSpace(dialog.NewShortcut?.Name) &&
                string.IsNullOrWhiteSpace(dialog.NewShortcut?.ProcessFile.Path))
            {
                return;
            }
            if (!File.Exists(dialog.NewShortcut.ProcessFile.Path) &&
                !Directory.Exists(dialog.NewShortcut.ProcessFile.Path) &&
                !LinkHelper.IsHyperlink(dialog.NewShortcut.ProcessFile.Path))
            {
                if (Popup.Show(
                    "This path does not exist.\n\nDo you want to add this shortcut anyway?",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.No)
                {
                    return;
                }
            }
            viewModel.Add(dialog.NewShortcut);
        }

        public static void NewSeparator(this ViewModel viewModel)
        {
            viewModel.Add(new Shortcut { Name = "Separator", SpecialType = "Separator" }, false);
        }

        public static void ClearIconCache(this ViewModel viewModel)
        {
            if (viewModel.Settings.UseIconCache)
            {
                foreach (var t in viewModel.Settings.Shortcuts)
                {
                    viewModel.IconCache.Remove(t.ProcessFile.Path);
                }
            }
            else
            {
                viewModel.IconCache.Clear();
            }
        }

        public static void ForceRefresh(this ViewModel viewModel)
        {
            viewModel.ClearIconCache();
        }

        public static void Execute(this ViewModel viewModel, Shortcut shortcut, bool hide = true)
        {
            if (viewModel.Settings.HideOnExecute && hide && viewModel.Settings.OpenMode != OpenMode.AlwaysOpen)
            {
                viewModel.View?.HideUi();
            }
            try
            {
                ProcessHelper.Launch(shortcut.ProcessFile);
            }
            catch (Win32Exception)
            {
                if (Popup.Show(
                    $"This file does not exist.\n\nDo you want to remove \"{GetName(shortcut)}\"?",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes) == MessageBoxResult.No)
                {
                    return;
                }
                viewModel.Remove(shortcut);
            }
        }

        public static void OpenProperties(this ViewModel viewModel, Shortcut shortcut)
        {
            var dialog = new ShortcutProperties(shortcut);
            dialog.ShowDialog();
            if (dialog.NewShortcut == null)
            {
                return;
            }
            viewModel.Settings.Shortcuts[viewModel.Settings.Shortcuts.IndexOf(shortcut)] = dialog.NewShortcut;
            viewModel.ReloadShortcutHotKey(dialog.NewShortcut);
        }

        public static void Remove(this ViewModel viewModel, Shortcut shortcut, bool msg = false)
        {
            if (msg &&
                Popup.Show($"Are you sure you want to remove \"{GetName(shortcut)}\"?",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes) == MessageBoxResult.No)
            {
                return;
            }
            viewModel.Settings.Shortcuts.Remove(shortcut);
            if (shortcut.Hotkey.Key != Key.None)
            {
                HotkeyStore.RemoveHotkey(shortcut.Hotkey.Guid);
            }
        }

        public static Shortcut MoveUp(this ViewModel viewModel, Shortcut shortcut, bool toEnd = false)
            => toEnd ? viewModel.Settings.Shortcuts.MoveToTop(shortcut) : viewModel.Settings.Shortcuts.MoveUp(shortcut);

        public static Shortcut MoveDown(this ViewModel viewModel, Shortcut shortcut, bool toEnd = false)
            =>
                toEnd
                    ? viewModel.Settings.Shortcuts.MoveToBottom(shortcut)
                    : viewModel.Settings.Shortcuts.MoveDown(shortcut);

        private static ImageSource GetShortcutIcon(Shortcut shortcut)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(shortcut.IconPath) && File.Exists(shortcut.IconPath))
                {
                    var bmi = new BitmapImage();
                    bmi.BeginInit();
                    bmi.CacheOption = BitmapCacheOption.OnLoad;
                    bmi.UriSource = new Uri(shortcut.IconPath, UriKind.Absolute);
                    bmi.EndInit();
                    return bmi;
                }
                if (File.Exists(shortcut.ProcessFile.Path) || Directory.Exists(shortcut.ProcessFile.Path))
                {
                    return IconHelper.GetPathIcon(shortcut.ProcessFile.Path);
                }
                if (shortcut.SpecialType == "Help")
                {
                    return SystemIcons.Information.ToImageSource();
                }
                if (LinkHelper.IsHyperlink(shortcut.ProcessFile.Path))
                {
                    return IconHelper.Extract("shell32.dll", 13, true).ToImageSource();
                }
            }
            catch
            {
                // ignored
            }
            return SystemIcons.Error.ToImageSource();
        }

        public static ImageSource GetShortcutIcon(this Shortcut shortcut, ViewModel viewModel)
        {
            if (!viewModel.Settings.UseIconCache)
            {
                return GetShortcutIcon(shortcut);
            }

            if (!viewModel.IconCache.ContainsKey(shortcut.ProcessFile.Path))
            {
                viewModel.IconCache.Add(shortcut.ProcessFile.Path, GetShortcutIcon(shortcut));
            }
            return viewModel.IconCache[shortcut.ProcessFile.Path];
        }

        private static IEnumerable<Shortcut> GetDefaultPresetShortcuts()
        {
            yield return new Shortcut
            {
                Name = "Help",
                ProcessFile = new ProcessFile(Resources.Website),
                SpecialType = "Help"
            };
            yield return new Shortcut
            {
                Name = "File Explorer",
                ProcessFile =
                    new ProcessFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows),
                        "explorer.exe"))
            };
            yield return new Shortcut
            {
                Name = "Notepad",
                ProcessFile =
                    new ProcessFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows),
                        "notepad.exe"))
            };
            yield return new Shortcut
            {
                Name = "Calculator",
                ProcessFile =
                    new ProcessFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System),
                        "calc.exe"))
            };
            yield return new Shortcut
            {
                Name = "Paint",
                ProcessFile =
                    new ProcessFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System),
                        "mspaint.exe"))
            };
        }

        private static IEnumerable<Shortcut> GetTaskbarShortcuts()
        {
            var taskbarPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                Resources.TaskBarPath);
            if (!Directory.Exists(taskbarPath))
            {
                yield break;
            }
            foreach (var shortcut in Directory.EnumerateFiles(taskbarPath)
                .Where(file => Path.GetFileName(file) != Resources.WindowsFolderDataFile)
                .Select(file => new Shortcut
                {
                    ProcessFile = new ProcessFile(file),
                    Name = Path.GetFileNameWithoutExtension(file)
                }))
            {
                yield return shortcut;
            }
        }

        public static IEnumerable<Shortcut> GetDefaultShortcuts(DefaultShortcutsMode mode)
        {
            switch (mode)
            {
                case DefaultShortcutsMode.Preset:
                    foreach (var shortcut in GetDefaultPresetShortcuts())
                    {
                        yield return shortcut;
                    }
                    break;
                case DefaultShortcutsMode.Taskbar:
                    foreach (var shortcut in GetTaskbarShortcuts())
                    {
                        yield return shortcut;
                    }
                    break;
            }
        }

        public static string GetFriendlyNameWithData(this Shortcut shortcut)
        {
            var dataList = new List<string>();
            if (!string.IsNullOrWhiteSpace(shortcut.ProcessFile.Path))
            {
                dataList.Add(shortcut.ProcessFile.Path);
            }
            if (!string.IsNullOrWhiteSpace(shortcut.ProcessFile.Arguments))
            {
                dataList.Add(shortcut.ProcessFile.Arguments);
            }
            var dataStr = string.Join(", ", dataList);
            if (!string.IsNullOrWhiteSpace(dataStr))
            {
                dataStr = $" ({dataStr})";
            }

            return $"{shortcut.Name}{dataStr}";
        }
    }
}