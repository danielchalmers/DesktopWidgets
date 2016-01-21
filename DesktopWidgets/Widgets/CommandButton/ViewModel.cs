using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using DesktopWidgets.Classes;
using DesktopWidgets.Helpers;
using DesktopWidgets.ViewModelBase;
using GalaSoft.MvvmLight.CommandWpf;

namespace DesktopWidgets.Widgets.CommandButton
{
    public class ViewModel : WidgetViewModelBase
    {
        public ViewModel(WidgetId id) : base(id)
        {
            Settings = id.GetSettings() as Settings;
            if (Settings == null)
                return;
            Execute = new RelayCommand(ExecuteCommand);
        }

        public Settings Settings { get; }

        public ICommand Execute { get; set; }

        public void ExecuteCommand()
        {
            if (string.IsNullOrWhiteSpace(Settings.Path))
            {
                Popup.Show("You must enter a file path to execute first.", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!File.Exists(Settings.Path))
            {
                Popup.Show("File path does not exist.", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Process.Start(Settings.Path, Settings.Args);
        }

        public override void ReloadHotKeys()
        {
            base.ReloadHotKeys();
            if (Settings.HotKey != Key.None)
                HotkeyStore.RegisterHotkey(_id.Guid,
                    new Hotkey(Settings.HotKey, Settings.HotKeyModifiers, Settings.FullscreenActivation), ExecuteCommand);
            else
                HotkeyStore.RemoveHotkey(_id.Guid);
        }
    }
}