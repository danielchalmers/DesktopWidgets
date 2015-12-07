using System;
using System.Windows;
using System.Windows.Interop;
using DesktopWidgets.Classes;
using DesktopWidgets.Helpers;
using DesktopWidgets.ViewModelBase;

namespace DesktopWidgets.View
{
    /// <summary>
    ///     Interaction logic for View.xaml
    /// </summary>
    public partial class WidgetView : Window
    {
        private readonly WidgetSettingsBase Settings;

        public WidgetView(WidgetId id)
        {
            InitializeComponent();
            Id = id;
            Settings = id.GetSettings();
        }

        public WidgetId Id { get; private set; }
        public bool AnimationRunning { get; set; } = false;
        private WidgetViewModelBase ViewModel => ((WidgetViewModelBase) DataContext);

        public void UpdateUi()
        {
            ViewModel.UpdateUi();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var hwnd = new WindowInteropHelper(this).Handle;
            if (Settings.Unclickable)
                Win32Helper.SetWindowExTransparent(hwnd);
        }
    }
}