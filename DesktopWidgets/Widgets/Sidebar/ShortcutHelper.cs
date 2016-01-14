#region

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DesktopWidgets.Classes;
using DesktopWidgets.Helpers;
using DesktopWidgets.Properties;

#endregion

namespace DesktopWidgets.Widgets.Sidebar
{
    public static class ShortcutHelper
    {
        public static string GetName(Shortcut shortcut)
            => string.IsNullOrWhiteSpace(shortcut.Name) ? shortcut.Path : shortcut.Name;

        private static string GetNameFromPath(string path)
        {
            var root = Path.GetPathRoot(path);
            if (path == root)
                return root;
            if (File.Exists(path))
                return Path.GetFileNameWithoutExtension(path);
            return path;
        }

        public static void ProcessFile(this ViewModel viewModel, string filepath, string name = "", bool msg = true)
        {
            if (viewModel.Settings.ParseShortcutFiles && Path.GetExtension(filepath) == ".lnk")
                filepath = FileSystemHelper.GetShortcutTargetFile(filepath);
            viewModel.Add(new Shortcut
            {
                Name = name == "" ? GetNameFromPath(filepath) : name,
                Path = filepath
            }, msg);
        }

        public static void ProcessFiles(this ViewModel viewModel, string[] files, bool msg = true)
        {
            if (files.Length >= 5 && msg &&
                Popup.Show($"You are attempting to add {files.Length} shortcuts. Are you sure?",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes) == MessageBoxResult.No)
                return;
            foreach (var file in files)
                viewModel.ProcessFile(file);
        }

        public static void Add(this ViewModel viewModel, Shortcut shortcut, bool msg = true)
        {
            // If shortcut path already exists, ask user if they are sure.
            if (msg && viewModel.Settings.Shortcuts.Any(x => x.Path == shortcut.Path))
                if (
                    Popup.Show(
                        "A shortcut with this path already exists.\n\nAre you sure you want to add this shortcut?",
                        MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes) == MessageBoxResult.No)
                    return;
            // Add new shortcut.
            viewModel.Settings.Shortcuts.Add(shortcut);
        }

        public static void OpenFolder(this Shortcut shortcut)
        {
            ProcessHelper.OpenFolder(shortcut.Path);
        }

        public static void Reload(this ViewModel viewModel)
        {
            if (Popup.Show("Are you sure you want to reload all shortcuts?",
                MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes) == MessageBoxResult.No)
                return;
            var shortcutList = viewModel.Settings.Shortcuts.ToList();
            viewModel.Settings.Shortcuts.Clear();
            foreach (var shortcut in shortcutList)
                if (shortcut.SpecialType == string.Empty)
                    viewModel.ProcessFile(shortcut.Path, shortcut.Name, false);
                else
                    viewModel.Add(shortcut, false);
            viewModel.ClearIconCache();
        }

        public static void New(this ViewModel viewModel)
        {
            var dialog = new ShortcutProperties();
            dialog.ShowDialog();
            if (string.IsNullOrWhiteSpace(dialog.NewShortcut?.Path))
                return;
            if (!File.Exists(dialog.NewShortcut.Path) && !Directory.Exists(dialog.NewShortcut.Path) &&
                !LinkHelper.IsHyperlink(dialog.NewShortcut.Path))
                if (Popup.Show(
                    "That path does not exist. Do you want to add this shortcut anyway?",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.No)
                    return;
            viewModel.Add(dialog.NewShortcut);
        }

        public static void NewSeparator(this ViewModel viewModel)
        {
            viewModel.Add(new Shortcut {Name = "Separator", SpecialType = "Separator"}, false);
        }

        public static void ClearIconCache(this ViewModel viewModel)
        {
            if (viewModel.Settings.UseIconCache)
                foreach (var t in viewModel.Settings.Shortcuts)
                    viewModel.IconCache.Remove(t.Path);
            else
                viewModel.IconCache.Clear();
        }

        public static void ForceRefresh(this ViewModel viewModel)
        {
            viewModel.ClearIconCache();
            //viewModel.Refresh();
        }

        public static void Execute(this ViewModel viewModel, Shortcut shortcut, bool hide = true)
        {
            if (viewModel.Settings.HideOnExecute && hide && viewModel.Settings.OpenMode != OpenMode.AlwaysOpen)
                viewModel._id.GetView()?.HideUI();
            if (File.Exists(shortcut.Path) || Directory.Exists(shortcut.Path) || LinkHelper.IsHyperlink(shortcut.Path))
            {
                ProcessHelper.Launch(shortcut.Path, shortcut.Args, shortcut.StartInFolder, shortcut.WindowStyle);
            }
            else
            {
                if (Popup.Show(
                    $"This file does not exist. Do you want to remove \"{GetName(shortcut)}\"?",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes) == MessageBoxResult.No)
                    return;
                viewModel.Remove(shortcut);
            }
        }

        public static void OpenProperties(this ViewModel viewModel, Shortcut shortcut)
        {
            var dialog = new ShortcutProperties(shortcut);
            dialog.ShowDialog();
            if (dialog.NewShortcut == null)
                return;
            viewModel.Settings.Shortcuts[viewModel.Settings.Shortcuts.IndexOf(shortcut)] = dialog.NewShortcut;
            viewModel.ReloadShortcutHotKey(dialog.NewShortcut);
        }

        public static void Remove(this ViewModel viewModel, Shortcut shortcut, bool msg = false)
        {
            if (msg &&
                Popup.Show($"Are you sure you want to remove \"{GetName(shortcut)}\"?",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes) == MessageBoxResult.No)
                return;
            viewModel.Settings.Shortcuts.Remove(shortcut);
            if (shortcut.HotKey != Key.None)
                HotkeyStore.RemoveHotkey(shortcut.Guid);
        }

        public static void Reset(this ViewModel viewModel, bool msg = false)
        {
            if (msg &&
                Popup.Show("Are you sure you want to remove all shortcuts? This cannot be undone.",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes) == MessageBoxResult.No)
                return;
            viewModel.Settings.Shortcuts.Clear();
        }

        public static void PopulateListBox(this ViewModel viewModel, ref ListBox listBox)
        {
            listBox.Items.Clear();
            foreach (var shortcut in viewModel.Settings.Shortcuts)
                listBox.Items.Add(shortcut.Name);
        }

        public static object MoveUp(this ViewModel viewModel, Shortcut shortcut, bool toEnd = false)
        {
            var index = viewModel.Settings.Shortcuts.IndexOf(shortcut);
            if (toEnd)
            {
                return viewModel.Settings.Shortcuts.Swap(index, 0);
            }
            if (index == 0)
                return viewModel.MoveDown(shortcut, true);
            return viewModel.Settings.Shortcuts.Swap(index, index - 1);
        }

        public static object MoveDown(this ViewModel viewModel, Shortcut shortcut, bool toEnd = false)
        {
            var index = viewModel.Settings.Shortcuts.IndexOf(shortcut);
            if (toEnd)
            {
                return viewModel.Settings.Shortcuts.Swap(index, viewModel.Settings.Shortcuts.Count - 1);
            }
            if (viewModel.Settings.Shortcuts.Count - 1 < index + 1)
                return viewModel.MoveUp(shortcut, true);
            return viewModel.Settings.Shortcuts.Swap(index, index + 1);
        }

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
                if (File.Exists(shortcut.Path) || Directory.Exists(shortcut.Path))
                    return IconHelper.GetPathIcon(shortcut.Path);
                if (shortcut.SpecialType == "Help")
                    return SystemIcons.Information.ToImageSource();
                if (LinkHelper.IsHyperlink(shortcut.Path))
                    return IconUtilities.Extract("shell32.dll", 13, true).ToImageSource();
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
                return GetShortcutIcon(shortcut);

            if (!viewModel.IconCache.ContainsKey(shortcut.Path))
                viewModel.IconCache.Add(shortcut.Path, GetShortcutIcon(shortcut));
            return viewModel.IconCache[shortcut.Path];
        }

        public static IEnumerable<Shortcut> GetDefaultShortcuts(DefaultShortcutsMode mode)
        {
            var defaults = new List<Shortcut>();

            switch (mode)
            {
                default:
                case DefaultShortcutsMode.DontChange:
                    break;
                case DefaultShortcutsMode.Preset:
                    defaults.Add(new Shortcut
                    {
                        Name = "Help",
                        Path = Resources.Website,
                        SpecialType = "Help"
                    });
                    defaults.Add(new Shortcut
                    {
                        Name = "File Explorer",
                        Path = Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\explorer.exe"
                    });
                    defaults.Add(new Shortcut
                    {
                        Name = "Notepad",
                        Path = Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\notepad.exe"
                    });
                    defaults.Add(new Shortcut
                    {
                        Name = "Calculator",
                        Path = Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\calc.exe"
                    });
                    defaults.Add(new Shortcut
                    {
                        Name = "Command Prompt",
                        Path = Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\cmd.exe"
                    });
                    break;
                case DefaultShortcutsMode.Taskbar:
                    try
                    {
                        var taskbarPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                          Resources.TaskBarPath;
                        defaults.AddRange(
                            Directory.GetFiles(taskbarPath)
                                .Where(file => Path.GetFileName(file) != "desktop.ini")
                                .Select(
                                    file => new Shortcut {Path = file, Name = Path.GetFileNameWithoutExtension(file)}));
                    }
                    catch
                    {
                        // ignored
                    }
                    break;
            }

            return defaults;
        }

        public static string GetFriendlyNameWithData(this Shortcut shortcut)
        {
            var dataList = new List<string>();
            if (!string.IsNullOrWhiteSpace(shortcut.Path))
                dataList.Add(shortcut.Path);
            if (!string.IsNullOrWhiteSpace(shortcut.Args))
                dataList.Add(shortcut.Args);
            var dataStr = string.Join(", ", dataList);
            if (!string.IsNullOrWhiteSpace(dataStr))
                dataStr = $" ({dataStr})";

            return $"{shortcut.Name}{dataStr}";
        }
    }
}