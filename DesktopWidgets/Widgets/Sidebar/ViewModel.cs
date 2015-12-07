using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using DesktopWidgets.Classes;
using DesktopWidgets.Helpers;
using DesktopWidgets.ViewModelBase;
using GalaSoft.MvvmLight.Command;

namespace DesktopWidgets.Widgets.Sidebar
{
    public class ViewModel : WidgetViewModelBase
    {
        public ViewModel(WidgetId id) : base(id)
        {
            Settings = id.GetSettings() as Settings;
            if (Settings == null)
                return;
            IconCache = new Dictionary<string, ImageSource>();

            Drop = new RelayCommand<System.Windows.DragEventArgs>(DropExecute);
            Refresh = new RelayCommand(RefreshExecute);
            NewShortcut = new RelayCommand(NewShortcutExecute);
            ManageShortcut = new RelayCommand(ManageShortcutsExecute);

            ShortcutPreviewMouseLeftButtonUp = new RelayCommand<Shortcut>(SetFocus);

            ShortcutEdit = new RelayCommand(ShortcutEditExecute);
            ShortcutMoveUp = new RelayCommand(ShortcutMoveUpExecute);
            ShortcutMoveDown = new RelayCommand(ShortcutMoveDownExecute);
            ShortcutRemove = new RelayCommand(ShortcutRemoveExecute);
            ShortcutOpenFolder = new RelayCommand(ShortcutOpenFolderExecute);
            ShortcutExecute = new RelayCommand<Shortcut>(ShortcutExecuteExecute);

            if (Settings.DefaultShortcutsMode != DefaultShortcutsMode.DontChange)
            {
                Settings.Shortcuts = new ObservableCollection<Shortcut>(ShortcutHelper.GetDefaultShortcuts(Settings.DefaultShortcutsMode));
                Settings.DefaultShortcutsMode = DefaultShortcutsMode.DontChange;
            }
        }

        public Settings Settings { get; }

        public Dictionary<string, ImageSource> IconCache { get; set; }

        public ICommand Refresh { get; set; }
        public ICommand Drop { get; set; }

        public ICommand ShortcutPreviewMouseLeftButtonUp { get; set; }

        public ICommand ShortcutEdit { get; set; }
        public ICommand ShortcutMoveUp { get; set; }
        public ICommand ShortcutMoveDown { get; set; }
        public ICommand ShortcutRemove { get; set; }
        public ICommand ShortcutOpenFolder { get; set; }
        public ICommand NewShortcut { get; set; }
        public ICommand ManageShortcut { get; set; }
        public ICommand ShortcutExecute { get; set; }

        private Shortcut SelectedShortcut { get; set; }

        private void DropExecute(System.Windows.DragEventArgs e)
        {
            if (Settings.AllowDropFiles && e.Data.GetDataPresent(DataFormats.FileDrop))
                this.ProcessFiles((string[]) e.Data.GetData(DataFormats.FileDrop));
        }

        private void SetFocus(Shortcut shortcut)
        {
            SelectedShortcut = shortcut;
        }

        private void ShortcutExecuteExecute(Shortcut shortcut)
        {
            SetFocus(shortcut);
            this.Execute(SelectedShortcut, !Keyboard.Modifiers.HasFlag(ModifierKeys.Shift));
        }

        private void ShortcutEditExecute()
        {
            this.OpenProperties(SelectedShortcut);
        }

        private void ShortcutMoveUpExecute()
        {
            this.MoveUp(SelectedShortcut);
        }

        private void ShortcutMoveDownExecute()
        {
            this.MoveDown(SelectedShortcut);
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

        private void ManageShortcutsExecute()
        {
            var dialog = new ManageShortcuts(this);
            dialog.ShowDialog();
        }

        private void RefreshExecute()
        {
            this.ForceRefresh();
        }
    }
}