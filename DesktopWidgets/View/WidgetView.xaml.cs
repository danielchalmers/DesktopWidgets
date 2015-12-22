using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;
using DesktopWidgets.Classes;
using DesktopWidgets.Helpers;
using DesktopWidgets.ViewModelBase;
using NHotkey;
using NHotkey.Wpf;

namespace DesktopWidgets.View
{
    /// <summary>
    ///     Interaction logic for View.xaml
    /// </summary>
    public partial class WidgetView : Window
    {
        private readonly DispatcherTimer _introTimer;
        private readonly MouseChecker _mouseChecker;
        public readonly WidgetSettingsBase Settings;
        public readonly UserControl UserControl;
        public readonly WidgetViewModelBase ViewModel;
        private DispatcherTimer _onTopForceTimer;

        public bool QueueIntro;

        public WidgetView(WidgetId id, WidgetViewModelBase viewModel, UserControl userControl)
        {
            InitializeComponent();
            Opacity = 0;
            Id = id;
            Settings = id.GetSettings();
            ViewModel = viewModel;
            UserControl = userControl;

            _introTimer = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(Settings.IntroDuration)};

            if (!App.Arguments.Contains("-systemstartup"))
                QueueIntro = true;

            userControl.Style = (Style) FindResource("UserControlStyle");
            MainContentContainer.Content = userControl;
            MainContentContainer.ContextMenu =
                (ContextMenu)
                    (userControl.TryFindResource("WidgetContextMenu") ?? TryFindResource("WidgetContextMenu"));
            userControl.MouseDown += OnMouseDown;

            _mouseChecker = new MouseChecker(this, Settings);
            UpdateUi(false);
            _mouseChecker.Start();
        }

        public bool IsContextMenuOpen => ViewModel.IsContextMenuOpen;

        public WidgetId Id { get; private set; }
        public bool AnimationRunning { get; set; } = false;

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var hwnd = new WindowInteropHelper(this).Handle;
            var widgetSrc = HwndSource.FromHwnd(hwnd);

            widgetSrc?.AddHook(WndProc);

            if (Settings.Unclickable)
                new Win32App(hwnd).SetWindowExTransparent();
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == SingleInstanceHelper.WM_SHOWAPP)
                ShowIntro();

            return IntPtr.Zero;
        }

        private void OnHotKey(object sender, HotkeyEventArgs e)
        {
            if (e.Name == "Show")
                ShowIntro();
            e.Handled = true;
        }

        public void ShowIntro()
        {
            if (Settings.OpenMode == OpenMode.AlwaysOpen || !Settings.ShowIntro)
                return;

            if (Opacity < 1)
            {
                QueueIntro = true;
                return;
            }
            QueueIntro = false;

            _introTimer.Stop();
            if (_mouseChecker.KeepOpenForIntro)
            {
                _mouseChecker.KeepOpenForIntro = false;
                _mouseChecker.Hide(checkHideStatus: true);
                _introTimer.Tick += delegate
                {
                    _introTimer.Stop();
                    _mouseChecker.KeepOpenForIntro = true;
                    _mouseChecker.Show();
                };
            }
            else
            {
                _mouseChecker.KeepOpenForIntro = true;
                _mouseChecker.Show();
                _introTimer.Tick += delegate
                {
                    _introTimer.Stop();
                    _mouseChecker.KeepOpenForIntro = false;
                    _mouseChecker.Hide(checkHideStatus: true);
                };
            }
            _introTimer.Start();
        }

        public void ShowUI()
        {
            _mouseChecker.Show();
        }

        public void HideUI()
        {
            _mouseChecker.Hide();
        }

        private void ReloadHotKeys()
        {
            try
            {
                if (Settings.OpenMode == OpenMode.Keyboard || Settings.OpenMode == OpenMode.MouseAndKeyboard)
                {
                    HotkeyManager.Current.AddOrReplace("Show", Settings.HotKey, Settings.HotKeyModifiers, OnHotKey);
                }
            }
            catch (HotkeyAlreadyRegisteredException)
            {
            }
        }


        public void UpdateUi(bool resetOpacity = true, ScreenDockPosition? dockPosition = null,
            ScreenDockAlignment? dockAlignment = null)
        {
            if (Opacity < 1)
                Refresh(resetOpacity);
            else
                this.Animate(AnimationMode.Hide, null, () => Refresh(), dockPosition, dockAlignment);
        }

        private void Refresh(bool resetOpacity = true)
        {
            DataContext = null;
            DataContext = ViewModel;
            UpdateLayout();
            ViewModel.UpdateSize();
            UpdateLayout();
            ViewModel.UpdatePosition();
            UpdateLayout();
            ViewModel.UpdatePosition();
            UpdateTimers();
            ReloadHotKeys();
            if (Opacity == 1 && !_mouseChecker.KeepOpenForIntro)
                ShowIntro();
            if (resetOpacity)
                Opacity = 1;
        }

        private void UpdateTimers()
        {
            _mouseChecker.UpdateIntervals();
            _mouseChecker.Stop();
            _mouseChecker.Start();
            if (Settings.ForceOnTop && Settings.ForceOnTopInterval > 0)
            {
                if (_onTopForceTimer == null)
                {
                    _onTopForceTimer = new DispatcherTimer();
                    _onTopForceTimer.Tick += delegate
                    {
                        Settings.OnTop = false;
                        Settings.OnTop = true;
                    };
                }
                _onTopForceTimer.Interval = TimeSpan.FromMilliseconds(Settings.ForceOnTopInterval);
                if (_onTopForceTimer.IsEnabled)
                {
                    _onTopForceTimer.Stop();
                    _onTopForceTimer.Start();
                }
            }
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed && Settings.DockPosition == ScreenDockPosition.None &&
                Settings.DragToMove)
                DragMove();
        }

        private void WidgetView_OnClosing(object sender, CancelEventArgs e)
        {
            _mouseChecker.Stop();
        }

        private void WidgetView_OnLocationChanged(object sender, EventArgs e)
        {
            if (Settings.SnapToScreenEdges)
                this.Snap();
        }
    }
}