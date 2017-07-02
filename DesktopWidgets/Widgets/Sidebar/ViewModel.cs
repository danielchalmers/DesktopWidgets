using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using DesktopWidgets.Helpers;
using DesktopWidgets.Stores;
using DesktopWidgets.WidgetBase;
using DesktopWidgets.WidgetBase.ViewModel;
using GalaSoft.MvvmLight.Command;
using DataFormats = System.Windows.Forms.DataFormats;

namespace DesktopWidgets.Widgets.Sidebar
{
    public class ViewModel : WidgetViewModelBase
    {
        public ViewModel(WidgetId id) : base(id)
        {
            Settings = id.GetSettings() as Settings;
            if (Settings == null)
            {
                return;
            }
            AllowDrop = Settings.AllowDropFiles;
            IconCache = new Dictionary<string, ImageSource>();

            Refresh = new RelayCommand(RefreshExecute);
            NewShortcut = new RelayCommand(NewShortcutExecute);
            NewSeparator = new RelayCommand(NewSeparatorExecute);

            ShortcutFocus = new RelayCommand<Shortcut>(ShortcutFocusExecute);

            ShortcutEdit = new RelayCommand(ShortcutEditExecute);
            ShortcutRemove = new RelayCommand(ShortcutRemoveExecute);
            ShortcutOpenFolder = new RelayCommand(ShortcutOpenFolderExecute);
            ShortcutExecute = new RelayCommand<Shortcut>(ShortcutExecuteExecute);

            if (Settings.DefaultShortcutsMode != DefaultShortcutsMode.DontChange)
            {
                Settings.Shortcuts =
                    new ObservableCollection<Shortcut>(ShortcutHelper.GetDefaultShortcuts(Settings.DefaultShortcutsMode));
                Settings.DefaultShortcutsMode = DefaultShortcutsMode.DontChange;
            }
        }

        public Settings Settings { get; }
        public Dictionary<string, ImageSource> IconCache { get; set; }
        public ICommand Refresh { get; set; }
        public ICommand ShortcutFocus { get; set; }
        public ICommand ShortcutEdit { get; set; }
        public ICommand ShortcutRemove { get; set; }
        public ICommand ShortcutOpenFolder { get; set; }
        public ICommand NewShortcut { get; set; }
        public ICommand NewSeparator { get; set; }
        public ICommand ShortcutExecute { get; set; }
        private Shortcut SelectedShortcut { get; set; }

        public override void OnRefresh()
        {
            base.OnRefresh();
            if (Settings?.Shortcuts != null)
            {
                foreach (var shortcut in Settings.Shortcuts.Where(x => x.Hotkey.Key != Key.None))
                {
                    ReloadShortcutHotKey(shortcut);
                }
            }
        }

        public void ReloadShortcutHotKey(Shortcut shortcut)
        {
            HotkeyStore.RegisterHotkey(shortcut.Hotkey,
                delegate { this.Execute(shortcut); });
        }

        private void ShortcutFocusExecute(Shortcut shortcut)
        {
            SelectedShortcut = shortcut;
        }

        private void ShortcutExecuteExecute(Shortcut shortcut)
        {
            ShortcutFocusExecute(shortcut);
            this.Execute(SelectedShortcut, !Keyboard.Modifiers.HasFlag(Settings.KeepOpenWithModifierKey));
        }

        private void ShortcutEditExecute()
        {
            this.OpenProperties(SelectedShortcut);
        }

        private void ShortcutRemoveExecute()
        {
            this.Remove(SelectedShortcut, true);
        }

        private void ShortcutOpenFolderExecute()
        {
            SelectedShortcut.OpenFolder();
        }

        private void NewShortcutExecute()
        {
            this.New();
        }

        private void NewSeparatorExecute()
        {
            this.NewSeparator();
        }

        private void RefreshExecute()
        {
            this.ForceRefresh();
        }

        public override void DropExecute(DragEventArgs e)
        {
            if (AllowDrop)
            {
                if (e.Data.GetDataPresent(DataFormats.Text))
                {
                    var text = (string)e.Data.GetData(DataFormats.Text);
                    if (LinkHelper.IsHyperlink(text))
                    {
                        this.ProcessFile(text);
                    }
                }
                else if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    this.ProcessFiles((string[])e.Data.GetData(DataFormats.FileDrop));
                }
            }
        }
    }
}