using System;
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
using WpfAppBar;

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

        private bool _isAppBar;

        private IntroData _lastIntroData;

        public Action CloseAction;

        public WidgetView(WidgetId id, WidgetViewModelBase viewModel, UserControl userControl, bool systemStartup)
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

            if (!systemStartup && Settings.ShowIntroOnLaunch)
                _mouseChecker.QueueIntro = new IntroData {ExecuteFinishAction = false};

            DataContext = ViewModel;

            _mouseChecker.Start();

            ViewModel.OnLoad();

            HasLoaded = true;
        }

        public bool HasLoaded { private get; set; }
        public bool HasSourceLoaded { private get; set; }

        public WidgetViewModelBase ViewModel { get; set; }

        public bool IsContextMenuOpen => ViewModel.IsContextMenuOpen;

        public WidgetId Id { get; }
        public bool AnimationRunning { get; set; }

        public new bool IsVisible => Visibility == Visibility.Visible && Opacity > 0.99;

        public bool IsIdle => Settings.DetectIdle && Settings.ActiveTimeEnd <= DateTime.Now;

        public bool IsClosed { get; set; }

        public Win32App ThisApp { get; set; }

        public bool IsRefreshing { get; set; }

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
            if (Settings.Style.ShowLeftFrame)
            {
                if (FrameContainerLeft.Child == null)
                {
                    var frameLeft = UserControl.TryFindResource("FrameLeft") as Grid;
                    if (frameLeft != null)
                    {
                        FrameContainerLeft.Child = frameLeft;
                        FrameContainerLeft.Visibility = Visibility.Visible;
                    }
                }
            }
            else
            {
                FrameContainerLeft.Visibility = Visibility.Collapsed;
            }

            if (Settings.Style.ShowRightFrame)
            {
                if (FrameContainerRight.Child == null)
                {
                    var frameRight = UserControl.TryFindResource("FrameRight") as Grid;
                    if (frameRight != null)
                    {
                        FrameContainerRight.Child = frameRight;
                        FrameContainerRight.Visibility = Visibility.Visible;
                    }
                }
            }
            else
            {
                FrameContainerRight.Visibility = Visibility.Collapsed;
            }

            if (Settings.Style.ShowTopFrame)
            {
                if (FrameContainerTop.Child == null)
                {
                    var frameTop = UserControl.TryFindResource("FrameTop") as Grid;
                    if (frameTop != null)
                    {
                        FrameContainerTop.Child = frameTop;
                        FrameContainerTop.Visibility = Visibility.Visible;
                    }
                }
            }
            else
            {
                FrameContainerTop.Visibility = Visibility.Collapsed;
            }

            if (Settings.Style.ShowBottomFrame)
            {
                if (FrameContainerBottom.Child == null)
                {
                    var frameBottom = UserControl.TryFindResource("FrameBottom") as Grid;
                    if (frameBottom != null)
                    {
                        FrameContainerBottom.Child = frameBottom;
                        FrameContainerBottom.Visibility = Visibility.Visible;
                    }
                }
            }
            else
            {
                FrameContainerBottom.Visibility = Visibility.Collapsed;
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

            if (!HasLoaded)
                return;

            var hwnd = new WindowInteropHelper(this).Handle;
            var widgetSrc = HwndSource.FromHwnd(hwnd);

            widgetSrc?.AddHook(WndProc);

            ThisApp = new Win32App(hwnd);

            if (Settings.Unclickable)
                ThisApp.SetWindowExTransparent();

            if (Settings.IsAppBar)
                SetAsAppBar();

            UpdateUi(false);

            ViewModel.OnUiLoad();

            //FocusMainElement();

            HasSourceLoaded = true;
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == SingleInstanceHelper.WM_SHOWAPP)
                ShowIntro();

            return IntPtr.Zero;
        }

        private void HideIntro(bool checkIdleStatus = true)
        {
            _mouseChecker.KeepOpenForIntro = false;
            if (Settings.OpenMode != OpenMode.AlwaysOpen)
                _mouseChecker.Hide(checkIdleStatus: checkIdleStatus);
        }

        public void CancelIntro()
        {
            if (!_introTimer.IsEnabled && !_mouseChecker.KeepOpenForIntro)
                return;
            HideIntro(false);
        }

        public void ShowIntro(IntroData introData = null)
        {
            _mouseChecker.QueueIntro = null;
            if (introData == null)
                introData = new IntroData();
            if (App.IsWorkstationLocked || FullScreenHelper.DoesMonitorHaveFullscreenApp(ViewModel.GetScreenBounds()))
            {
                _mouseChecker.QueueIntro = introData;
                return;
            }
            _lastIntroData = introData;
            if (_introTimer == null)
            {
                _introTimer = new DispatcherTimer();
                _introTimer.Tick += delegate
                {
                    _introTimer.Stop();
                    if (_lastIntroData.HideOnFinish)
                        HideIntro();
                    if (_lastIntroData.ExecuteFinishAction)
                        ViewModel.OnIntroEnd();
                };
            }

            _introTimer.Stop();
            _introTimer.Interval =
                TimeSpan.FromMilliseconds(introData.Duration == -1 ? Settings.IntroDuration : introData.Duration);
            if (_mouseChecker.KeepOpenForIntro && introData.Reversable)
            {
                HideIntro(false);
            }
            else if (introData.Duration != 0)
            {
                _mouseChecker.KeepOpenForIntro = true;
                _introTimer.Start();
                ViewModel.OnIntro();
                _mouseChecker.Show(activate: introData.Activate);
            }
        }

        public void ShowUi()
        {
            _mouseChecker.Show();
        }

        public void HideUi(bool checkIdleStatus = true, bool checkHideStatus = true)
        {
            if (Settings.OpenMode != OpenMode.AlwaysOpen)
                _mouseChecker.Hide(checkIdleStatus: checkIdleStatus, checkHideStatus: checkHideStatus);
        }

        public void Dismiss()
        {
            HideUi(false, false);
            ViewModel.OnDismiss();
        }

        public void UpdateUi(bool resetContext = true, bool updateNonUi = true, bool updateOpacity = true,
            bool? isDocked = null,
            HorizontalAlignment? dockHorizontalAlignment = null,
            VerticalAlignment? dockVerticalAlignment = null)
        {
            if (!IsVisible)
                Refresh(resetContext, updateNonUi, false, updateOpacity);
            else
                this.Animate(AnimationMode.Hide, false, null,
                    () => Refresh(resetContext, updateNonUi, true, updateOpacity),
                    isDocked,
                    dockHorizontalAlignment,
                    dockVerticalAlignment);
        }

        private void UpdatePositionAndLocation()
        {
            UpdateLayout();
            ViewModel.UpdateSize();
            UpdateLayout();
            ViewModel.UpdatePosition();
            UpdateLayout();
            ViewModel.UpdatePosition();
            UpdateLayout();
        }

        private void SetAsAppBar()
        {
            switch (Settings.HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    AppBarFunctions.SetAppBar(this, ABEdge.Left);
                    _isAppBar = true;
                    break;
                case HorizontalAlignment.Right:
                    AppBarFunctions.SetAppBar(this, ABEdge.Right);
                    _isAppBar = true;
                    break;
                default:
                    switch (Settings.VerticalAlignment)
                    {
                        case VerticalAlignment.Top:
                            AppBarFunctions.SetAppBar(this, ABEdge.Top);
                            _isAppBar = true;
                            break;
                        case VerticalAlignment.Bottom:
                            AppBarFunctions.SetAppBar(this, ABEdge.Bottom);
                            _isAppBar = true;
                            break;
                    }
                    break;
            }
        }

        private void Refresh(bool resetContext, bool updateNonUi, bool showIntro, bool updateOpacity)
        {
            IsRefreshing = true;
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

            SetupFrame();

            if (!_isAppBar)
            {
                UpdatePositionAndLocation();
            }

            if (updateNonUi)
            {
                UpdateTimers();
                ViewModel.ReloadTimers();
            }

            ViewModel.OnRefresh();

            if (!isVisible)
                Hide();
            if (updateOpacity)
                Opacity = 1;

            IsRefreshing = false;

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

        private void WidgetView_OnClosing(object sender, CancelEventArgs e)
        {
            try
            {
                IsClosed = true;
                _introTimer?.Stop();
                _introTimer = null;
                _mouseChecker?.Dispose();
                HotkeyStore.RemoveHotkey(Id.Guid);
                ViewModel?.OnClose();
                ViewModel = null;
                SaveScrollPosition();
            }
            catch
            {
                // ignored
            }

            App.WidgetViews.Remove(this);

            CloseAction?.Invoke();
        }

        private void WidgetView_OnLocationChanged(object sender, EventArgs e)
        {
            if (HasSourceLoaded && !IsRefreshing && Settings.SnapToScreenEdges && !Settings.IsDocked)
            {
                var newLoc = this.Snap(true, Settings.IgnoreAppBars);
                ViewModel.Left = newLoc.X;
                ViewModel.Top = newLoc.Y;
            }
        }

        private void Widget_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed && !Settings.IsDocked &&
                Settings.DragToMove)
                DragMove();
        }

        private void ActionBar_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed && !Settings.IsDocked &&
                Settings.DragActionBarToMove)
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
            Dismiss();
        }

        public void FocusMainElement()
        {
            GetMainElement()?.Focus();
        }

        public UIElement GetMainElement()
        {
            return UserControl.FindName("MainElement") as UIElement;
        }

        public void CloseAnimation()
        {
            this.Animate(AnimationMode.Hide, false, null, Close);
        }

        private void WidgetView_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!IsRefreshing && !_isAppBar)
                Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action) ViewModel.UpdatePosition);
        }

        public void ExecuteSpecialAction()
        {
            ViewModel.ExecuteSpecialAction();
        }
    }
}