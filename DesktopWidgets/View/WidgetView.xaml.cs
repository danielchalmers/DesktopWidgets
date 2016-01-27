﻿using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;
using DesktopWidgets.Classes;
using DesktopWidgets.Helpers;
using DesktopWidgets.Stores;
using DesktopWidgets.WidgetBase;
using DesktopWidgets.WidgetBase.Settings;
using DesktopWidgets.WidgetBase.ViewModel;

namespace DesktopWidgets.View
{
    /// <summary>
    ///     Interaction logic for View.xaml
    /// </summary>
    public partial class WidgetView : Window
    {
        private readonly MouseChecker _mouseChecker;
        public readonly WidgetSettingsBase Settings;
        public readonly UserControl UserControl;
        private DispatcherTimer _introTimer;

        public WidgetView(WidgetId id, WidgetViewModelBase viewModel, UserControl userControl)
        {
            InitializeComponent();

            Opacity = 0;
            Hide();

            Id = id;
            Settings = id.GetSettings();
            ViewModel = viewModel;
            UserControl = userControl;

            SetupWidgetControl();

            SetScrollPosition();

            SetupFrame();

            ViewModel.View = this;

            _mouseChecker = new MouseChecker(this, Settings);

            if (!App.Arguments.Contains("-systemstartup"))
                _mouseChecker.QueueIntro = true;

            DataContext = ViewModel;

            _mouseChecker.Start();

            ViewModel.OnLoad();
        }

        public WidgetViewModelBase ViewModel { get; set; }

        public bool IsContextMenuOpen => ViewModel.IsContextMenuOpen;

        public WidgetId Id { get; }
        public bool AnimationRunning { get; set; }

        public new bool IsVisible => Visibility == Visibility.Visible && Opacity > 0.99;

        private void SetupWidgetControl()
        {
            UserControl.Style = (Style) Resources["UserControlStyle"];
            MainContentContainer.Content = UserControl;
            var contextMenu = (ContextMenu)
                (UserControl.TryFindResource("WidgetContextMenu") ?? TryFindResource("WidgetContextMenu"));
            contextMenu.DataContext = ViewModel;
            MainContentContainer.ContextMenu = contextMenu;
            UserControl.MouseDown += Widget_OnMouseDown;
        }

        private void SetupFrame()
        {
            var frameTop = UserControl.TryFindResource("FrameTop") as Grid;
            if (frameTop != null)
            {
                FrameContainerTop.Child = frameTop;
                FrameContainerTop.Visibility = Visibility.Visible;
            }
            var frameBottom = UserControl.TryFindResource("FrameBottom") as Grid;
            if (frameBottom != null)
            {
                FrameContainerBottom.Child = frameBottom;
                FrameContainerBottom.Visibility = Visibility.Visible;
            }
            var frameLeft = UserControl.TryFindResource("FrameLeft") as Grid;
            if (frameLeft != null)
            {
                FrameContainerLeft.Child = frameLeft;
                FrameContainerLeft.Visibility = Visibility.Visible;
            }
            var frameRight = UserControl.TryFindResource("FrameRight") as Grid;
            if (frameRight != null)
            {
                FrameContainerRight.Child = frameRight;
                FrameContainerRight.Visibility = Visibility.Visible;
            }
        }

        private void SetScrollPosition()
        {
            MainContentContainer.ScrollToHorizontalOffset(Settings.ScrollHorizontalOffset);
            MainContentContainer.ScrollToVerticalOffset(Settings.ScrollVerticalOffset);
        }

        private void SaveScrollPosition()
        {
            Settings.ScrollHorizontalOffset = MainContentContainer.ContentHorizontalOffset;
            Settings.ScrollVerticalOffset = MainContentContainer.ContentVerticalOffset;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var hwnd = new WindowInteropHelper(this).Handle;
            var widgetSrc = HwndSource.FromHwnd(hwnd);

            widgetSrc?.AddHook(WndProc);

            if (Settings.Unclickable)
                new Win32App(hwnd).SetWindowExTransparent();

            UpdateUi(false, false);

            ViewModel.OnUiLoad();
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == SingleInstanceHelper.WM_SHOWAPP)
                ShowIntro();

            return IntPtr.Zero;
        }

        public void ShowIntro(int duration = -1, bool reversable = false, bool activate = false,
            bool hideOnFinish = true)
        {
            if (_introTimer == null)
            {
                _introTimer = new DispatcherTimer();
                _introTimer.Tick += delegate
                {
                    _introTimer.Stop();
                    if (hideOnFinish)
                    {
                        _mouseChecker.KeepOpenForIntro = false;
                        if (Settings.OpenMode != OpenMode.AlwaysOpen)
                            _mouseChecker.Hide(checkHideStatus: true);
                    }
                    ViewModel.OnIntroEnd();
                };
            }

            _introTimer.Stop();
            _introTimer.Interval = TimeSpan.FromMilliseconds(duration == -1 ? Settings.IntroDuration : duration);
            if (_mouseChecker.KeepOpenForIntro && reversable)
            {
                _mouseChecker.KeepOpenForIntro = false;
            }
            else if (duration != 0)
            {
                _mouseChecker.KeepOpenForIntro = true;
                _introTimer.Start();
                if (activate)
                    Activate();
                ViewModel.OnIntro();
            }
        }

        public void ShowUi()
        {
            if (Settings.OpenMode != OpenMode.AlwaysOpen && !App.IsMuted)
                _mouseChecker.Show();
        }

        public void HideUi()
        {
            if (Settings.OpenMode != OpenMode.AlwaysOpen)
                _mouseChecker.Hide();
        }

        public void UpdateUi(bool resetContext = true, bool updateNonUi = true, bool updateOpacity = true,
            bool? isDocked = null,
            HorizontalAlignment? dockHorizontalAlignment = null,
            VerticalAlignment? dockVerticalAlignment = null)
        {
            if (!IsVisible)
                Refresh(resetContext, updateNonUi, false, updateOpacity);
            else
                this.Animate(AnimationMode.Hide, null, () => Refresh(resetContext, updateNonUi, true, updateOpacity),
                    isDocked,
                    dockHorizontalAlignment,
                    dockVerticalAlignment);
        }

        private void Refresh(bool resetContext, bool updateNonUi, bool showIntro, bool updateOpacity)
        {
            var isVisible = IsVisible;
            Opacity = 0;
            if (!isVisible)
                Show();

            if (resetContext)
            {
                //UpdateLayout();
                DataContext = null;
                //UpdateLayout();
                DataContext = ViewModel;
            }

            Title = Id.GetName();

            ViewModel.OnTop = Settings.OnTop;
            UpdateLayout();
            ViewModel.UpdateSize();
            UpdateLayout();
            ViewModel.UpdatePosition();
            UpdateLayout();
            ViewModel.UpdatePosition();
            UpdateLayout();
            if (updateNonUi)
            {
                UpdateTimers();
                ViewModel.ReloadTimers();
                ViewModel.ReloadHotKeys();
            }

            ViewModel.OnRefresh();

            if (!isVisible)
                Hide();
            if (updateOpacity)
                Opacity = 1;

            if (showIntro && !_mouseChecker.KeepOpenForIntro)
                ShowIntro();
        }

        private void UpdateTimers()
        {
            _mouseChecker.UpdateIntervals();
            if (_mouseChecker.IsRunning)
            {
                _mouseChecker.Stop();
                _mouseChecker.Start();
            }
        }

        public void ResetOnTop()
        {
            Topmost = true;
            Topmost = false;
        }

        private void WidgetView_OnClosing(object sender, CancelEventArgs e)
        {
            _introTimer?.Stop();
            _introTimer = null;
            _mouseChecker.Dispose();
            HotkeyStore.RemoveHotkey(Id.Guid);
            ViewModel.OnClose();
            ViewModel = null;
            SaveScrollPosition();
        }

        private void WidgetView_OnLocationChanged(object sender, EventArgs e)
        {
            if (Settings.SnapToScreenEdges && !Settings.IsDocked)
                this.Snap(true, Settings.IgnoreAppBars);
        }

        private void Widget_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed && !Settings.IsDocked &&
                Settings.DragToMove)
                DragMove();
        }

        private void Titlebar_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed && !Settings.IsDocked &&
                Settings.DragTitlebarToMove)
                DragMove();
        }

        private void btnMenu_OnClick(object sender, RoutedEventArgs e)
        {
            MainContentContainer.ContextMenu.IsOpen = true;
        }

        private void btnReload_OnClick(object sender, RoutedEventArgs e)
        {
            Id.Reload();
        }

        public new void Show()
        {
            Visibility = Visibility.Visible;
        }

        public new void Hide()
        {
            Visibility = Visibility.Hidden;
        }

        private void btnDismiss_OnClick(object sender, RoutedEventArgs e)
        {
            HideUi();
        }
    }
}