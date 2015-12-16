using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using DesktopWidgets.Classes;
using DesktopWidgets.Helpers;
using DesktopWidgets.Windows;
using GalaSoft.MvvmLight.Command;
using NHotkey;
using NHotkey.Wpf;

namespace DesktopWidgets.ViewModelBase
{
    public abstract class WidgetViewModelBase : GalaSoft.MvvmLight.ViewModelBase
    {
        private readonly WidgetId _id;
        private readonly DispatcherTimer _introTimer;
        private readonly MouseChecker _mouseChecker;
        private DispatcherTimer _onTopForceTimer;
        private readonly WidgetSettingsBase _settings;
        private double _actualHeight;
        private double _actualWidth;

        private double _height;

        private bool _isContextMenuOpen;

        private double _left;

        private double _opacity;

        private double _top;

        private double _width;

        public bool QueueIntro;

        protected WidgetViewModelBase(WidgetId id)
        {
            Opacity = 0;
            _id = id;
            _settings = id.GetSettings();
            MouseDown = new RelayCommand<Window>(OnMouseDownExecute);
            LocationChanged = new RelayCommand<Window>(OnLocationChangedExecute);
            Closing = new RelayCommand<Window>(OnClosingExecute);
            KeyDown = new RelayCommand<KeyEventArgs>(OnKeyDownExecute);
            EditWidget = new RelayCommand(EditWidgetExecute);
            ReloadWidget = new RelayCommand(ReloadWidgetExecute);
            ToggleEnableWidget = new RelayCommand(ToggleEnableWidgetExecute);
            ManageAllWidgets = new RelayCommand(ManageAllWidgetsExecute);

            WidgetBringToFront = new RelayCommand<Window>(WidgetBringToFrontExecute);
            WidgetDockPosition = new RelayCommand<ScreenDockPosition>(WidgetDockPositionExecute);
            WidgetDockAlignment = new RelayCommand<ScreenDockAlignment>(WidgetDockAlignmentExecute);

            _introTimer = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(_settings.IntroDuration)};
            if (!App.Arguments.Contains("-systemstartup"))
                QueueIntro = true;
            _mouseChecker = new MouseChecker(id, this);
            UpdateUi();
            _mouseChecker.Start();
        }

        public double Left
        {
            get { return _left; }
            set
            {
                if (_left != value)
                {
                    _left = value;
                    if (_settings.DockPosition == ScreenDockPosition.None)
                        _settings.Left = value;
                    RaisePropertyChanged(nameof(Left));
                }
            }
        }

        public double Top
        {
            get { return _top; }
            set
            {
                if (_top != value)
                {
                    _top = value;
                    if (_settings.DockPosition == ScreenDockPosition.None)
                        _settings.Top = value;
                    RaisePropertyChanged(nameof(Top));
                }
            }
        }

        public double Width
        {
            get { return _width; }
            set
            {
                if (_width != value)
                {
                    _width = value;
                    RaisePropertyChanged(nameof(Width));
                }
            }
        }

        public double Height
        {
            get { return _height; }
            set
            {
                if (_height != value)
                {
                    _height = value;
                    RaisePropertyChanged(nameof(Height));
                }
            }
        }

        public double MaxWidth
        {
            get { return _settings.AutoMaxSize ? MonitorHelper.GetMonitorBounds(_settings.Monitor).Width : _settings.MaxWidth; }
            set
            {
                if (_settings.MaxWidth != value)
                {
                    _settings.MaxWidth = value;
                    RaisePropertyChanged(nameof(MaxWidth));
                }
            }
        }

        public double MaxHeight
        {
            get { return _settings.AutoMaxSize ? MonitorHelper.GetMonitorBounds(_settings.Monitor).Height : _settings.MaxHeight; }
            set
            {
                if (_settings.MaxHeight != value)
                {
                    _settings.MaxHeight = value;
                    RaisePropertyChanged(nameof(MaxHeight));
                }
            }
        }

        public double ActualWidth
        {
            get { return _actualWidth; }
            set
            {
                if (_actualWidth != value)
                {
                    _actualWidth = value;
                    RaisePropertyChanged(nameof(ActualWidth));
                }
            }
        }

        public double ActualHeight
        {
            get { return _actualHeight; }
            set
            {
                if (_actualHeight != value)
                {
                    _actualHeight = value;
                    RaisePropertyChanged(nameof(ActualHeight));
                }
            }
        }

        public double Opacity
        {
            get { return _opacity; }
            set
            {
                if (_opacity != value)
                {
                    _opacity = value;
                    RaisePropertyChanged(nameof(Opacity));
                }
            }
        }

        public ICommand Closing { get; private set; }
        public ICommand MouseDown { get; private set; }
        public ICommand LocationChanged { get; private set; }
        public ICommand KeyDown { get; private set; }
        public ICommand EditWidget { get; private set; }
        public ICommand ReloadWidget { get; private set; }
        public ICommand ToggleEnableWidget { get; private set; }
        public ICommand ManageAllWidgets { get; private set; }

        public ICommand WidgetBringToFront { get; private set; }

        public ICommand WidgetDockPosition { get; private set; }
        public ICommand WidgetDockAlignment { get; private set; }

        public bool IsContextMenuOpen
        {
            get { return _isContextMenuOpen; }
            set
            {
                if (_isContextMenuOpen != value)
                {
                    _isContextMenuOpen = value;
                    RaisePropertyChanged(nameof(IsContextMenuOpen));
                }
            }
        }

        private void ReloadHotKeys()
        {
            try
            {
                if (_settings.OpenMode == OpenMode.Keyboard || _settings.OpenMode == OpenMode.MouseAndKeyboard)
                {
                    HotkeyManager.Current.AddOrReplace("Show", _settings.HotKey, _settings.HotKeyModifiers, OnHotKey);
                }
            }
            catch (HotkeyAlreadyRegisteredException)
            {
            }
        }

        private void OnHotKey(object sender, HotkeyEventArgs e)
        {
            if (e.Name == "Show")
                ShowIntro();
            e.Handled = true;
        }

        public void ShowIntro()
        {
            if (_settings.OpenMode == OpenMode.AlwaysOpen || !_settings.ShowIntro)
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

        private void UpdateTimers()
        {
            _mouseChecker.UpdateIntervals();
            _mouseChecker.Stop();
            _mouseChecker.Start();
            if (_settings.ForceOnTop && _settings.ForceOnTopInterval > 0)
            {
                if (_onTopForceTimer == null)
                {
                    _onTopForceTimer = new DispatcherTimer();
                    _onTopForceTimer.Tick += delegate
                    {
                        _settings.OnTop = false;
                        _settings.OnTop = true;
                    };
                }
                _onTopForceTimer.Interval = TimeSpan.FromMilliseconds(_settings.ForceOnTopInterval);
                if (_onTopForceTimer.IsEnabled)
                {
                    _onTopForceTimer.Stop();
                    _onTopForceTimer.Start();
                }
            }
        }

        public virtual void UpdateUi()
        {
            UpdatePosition();
            UpdateTimers();
            ReloadHotKeys();
        }

        private void UpdatePosition()
        {
            var newLeft = double.NaN;
            var newTop = double.NaN;
            var newWidth = double.NaN;
            var newHeight = double.NaN;
            newWidth = _settings.Width;
            newHeight = _settings.Height;
            if (_settings.DockPosition == ScreenDockPosition.None)
            {
                newLeft = _settings.Left;
                newTop = _settings.Top;
            }
            else
            {
                var horizontal = _settings.DockPosition.IsHorizontal();
                var monitorRect =
                    MonitorHelper.GetMonitorBounds(_settings.Monitor);

                if (horizontal)
                {
                    switch (_settings.DockAlignment)
                    {
                        case ScreenDockAlignment.Top:
                            newTop = monitorRect.Top +
                                     (_settings.IgnoreCorners ? (_settings.CornerSize*2) : 0);
                            break;
                        default:
                        case ScreenDockAlignment.Center:
                            newTop = monitorRect.Top + (monitorRect.Height/2) - (ActualHeight/2);
                            break;
                        case ScreenDockAlignment.Bottom:
                            newTop = (monitorRect.Bottom - ActualHeight) -
                                     (_settings.IgnoreCorners ? (_settings.CornerSize*2) : 0);
                            break;
                        case ScreenDockAlignment.Stretch:
                            newHeight = monitorRect.Height;
                            newTop = monitorRect.Top;
                            break;
                    }
                    newWidth = _settings.Width > _settings.MinWidth ? _settings.Width : _settings.MinWidth;
                    newLeft = _settings.DockPosition == ScreenDockPosition.Left
                        ? monitorRect.Left
                        : monitorRect.Right - ActualWidth;
                }
                else
                {
                    switch (_settings.DockAlignment)
                    {
                        case ScreenDockAlignment.Top:
                            newLeft = monitorRect.Left +
                                      (_settings.IgnoreCorners ? (_settings.CornerSize*2) : 0);
                            break;
                        default:
                        case ScreenDockAlignment.Center:
                            newLeft = monitorRect.Left + (monitorRect.Width/2) - (ActualWidth/2);
                            break;
                        case ScreenDockAlignment.Bottom:
                            newLeft = (monitorRect.Right - ActualWidth) -
                                      (_settings.IgnoreCorners ? (_settings.CornerSize*2) : 0);
                            break;
                        case ScreenDockAlignment.Stretch:
                            newWidth = monitorRect.Width;
                            newLeft = monitorRect.Left;
                            break;
                    }
                    newHeight = _settings.Width > _settings.MinHeight ? _settings.Width : _settings.MinHeight;
                    newTop = _settings.DockPosition == ScreenDockPosition.Top
                        ? monitorRect.Top
                        : monitorRect.Bottom - ActualHeight;
                }
                newLeft += _settings.DockOffset.X;
                newTop += _settings.DockOffset.Y;
            }
            Left = newLeft;
            Top = newTop;
            Width = newWidth;
            Height = newHeight;
        }

        private void OnClosingExecute(Window window)
        {
            _mouseChecker.Stop();
        }

        private void OnMouseDownExecute(Window window)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed && _settings.DockPosition == ScreenDockPosition.None &&
                _settings.DragToMove)
                window.DragMove();
        }

        private void OnLocationChangedExecute(Window window)
        {
            if (_settings.SnapToScreenEdges)
                window.Snap();
        }

        private void OnKeyDownExecute(KeyEventArgs e)
        {
            if (_settings.MoveHotkeys)
            {
                switch (e.Key)
                {
                    case Key.Up:
                        Top -= _settings.MoveDistance;
                        break;
                    case Key.Down:
                        Top += _settings.MoveDistance;
                        break;
                    case Key.Left:
                        Left -= _settings.MoveDistance;
                        break;
                    case Key.Right:
                        Left += _settings.MoveDistance;
                        break;
                }
            }
        }

        private void EditWidgetExecute()
        {
            _id.Edit();
        }

        private void ReloadWidgetExecute()
        {
            _id.ToggleEnable();
            _id.ToggleEnable();
        }

        private void ToggleEnableWidgetExecute()
        {
            _id.ToggleEnable();
        }

        private void ManageAllWidgetsExecute()
        {
            new ManageWidgets().Show();
        }

        private void WidgetBringToFrontExecute(Window window)
        {
            window.BringIntoView();
        }

        private void WidgetDockPositionExecute(ScreenDockPosition screenDockPosition)
        {
            _settings.DockPosition = screenDockPosition;
            UpdatePosition();
        }

        private void WidgetDockAlignmentExecute(ScreenDockAlignment screenDockAlignment)
        {
            _settings.DockAlignment = screenDockAlignment;
            UpdatePosition();
        }
    }
}