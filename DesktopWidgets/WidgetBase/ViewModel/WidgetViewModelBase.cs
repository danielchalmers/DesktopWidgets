using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using DesktopWidgets.Classes;
using DesktopWidgets.Helpers;
using DesktopWidgets.Stores;
using DesktopWidgets.View;
using DesktopWidgets.WidgetBase.Settings;
using DesktopWidgets.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace DesktopWidgets.WidgetBase.ViewModel
{
    public abstract class WidgetViewModelBase : ViewModelBase
    {
        private readonly WidgetSettingsBase _settings;
        protected readonly WidgetId Id;
        private double _actualHeight;
        private double _actualWidth;

        private double _height;

        private bool _isContextMenuOpen;

        private double _left;

        private bool _onTop;
        private DispatcherTimer _onTopForceTimer;

        private double _top;

        private double _width;

        protected WidgetViewModelBase(WidgetId id)
        {
            Id = id;
            _settings = id.GetSettings();
            DismissWidget = new RelayCommand(DismissWidgetExecute);
            EditWidget = new RelayCommand(EditWidgetExecute);
            ReloadWidget = new RelayCommand(ReloadWidgetExecute);
            ToggleEnableWidget = new RelayCommand(ToggleEnableWidgetExecute);
            ManageAllWidgets = new RelayCommand(ManageAllWidgetsExecute);

            Drop = new RelayCommand<DragEventArgs>(DropExecute);
            MouseMove = new RelayCommand<MouseEventArgs>(MouseMoveExecute);
            MouseDown = new RelayCommand<MouseEventArgs>(MouseDownExecute);
            MouseUp = new RelayCommand<MouseEventArgs>(MouseUpExecute);
            KeyDown = new RelayCommand<KeyEventArgs>(KeyDownExecute);
            KeyUp = new RelayCommand<KeyEventArgs>(KeyUpExecute);
            MouseDoubleClick = new RelayCommand<MouseButtonEventArgs>(MouseDoubleClickExecute);

            WidgetDockHorizontal = new RelayCommand<HorizontalAlignment>(WidgetDockHorizontalExecute);
            WidgetDockVertical = new RelayCommand<VerticalAlignment>(WidgetDockVerticalExecute);
            WidgetUndock = new RelayCommand(WidgetUndockExecute);
        }

        public bool AllowDrop { get; set; }

        public WidgetView View { get; set; }

        public bool OnTop
        {
            get { return _onTop; }
            set
            {
                if (_onTop != value)
                {
                    _onTop = value;
                    RaisePropertyChanged(nameof(OnTop));
                }
            }
        }

        public double Left
        {
            get { return _left; }
            set
            {
                if (value.IsEqual(_left))
                {
                    if (!_settings.IsDocked)
                        _settings.Left = value;
                    _left = value;
                    RaisePropertyChanged(nameof(Left));
                }
            }
        }

        public double Top
        {
            get { return _top; }
            set
            {
                if (value.IsEqual(_top))
                {
                    if (!_settings.IsDocked)
                        _settings.Top = value;
                    _top = value;
                    RaisePropertyChanged(nameof(Top));
                }
            }
        }

        public double Width
        {
            get { return _width; }
            set
            {
                if (value.IsEqual(_width))
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
                if (value.IsEqual(_height))
                {
                    _height = value;
                    RaisePropertyChanged(nameof(Height));
                }
            }
        }

        public double MaxWidth => double.IsNaN(_settings.MaxWidth) && _settings.MaxSizeUseScreen
            ? _settings.ScreenBounds.Width
            : _settings.MaxWidth;

        public double MaxHeight => double.IsNaN(_settings.MaxHeight) && _settings.MaxSizeUseScreen
            ? _settings.ScreenBounds.Height
            : _settings.MaxHeight;

        public double ActualWidth
        {
            get { return _actualWidth; }
            set
            {
                if (value > 0 && value.IsEqual(_actualWidth))
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
                if (value > 0 && value.IsEqual(_actualHeight))
                {
                    _actualHeight = value;
                    RaisePropertyChanged(nameof(ActualHeight));
                }
            }
        }

        public ICommand DismissWidget { get; private set; }
        public ICommand EditWidget { get; private set; }
        public ICommand ReloadWidget { get; private set; }
        public ICommand ToggleEnableWidget { get; private set; }
        public ICommand ManageAllWidgets { get; private set; }

        public ICommand WidgetDockHorizontal { get; private set; }
        public ICommand WidgetDockVertical { get; private set; }
        public ICommand WidgetUndock { get; private set; }

        public ICommand Drop { get; set; }
        public ICommand MouseMove { get; set; }
        public ICommand MouseDown { get; set; }
        public ICommand MouseUp { get; set; }
        public ICommand KeyDown { get; set; }
        public ICommand KeyUp { get; set; }
        public ICommand MouseDoubleClick { get; set; }

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

        private double GetLeft()
        {
            if (!_settings.IsDocked)
            {
                return _settings.Left;
            }
            var monitorRect = _settings.ScreenBounds;

            switch (_settings.HorizontalAlignment)
            {
                case HorizontalAlignment.Stretch:
                case HorizontalAlignment.Left:
                    return monitorRect.Left + _settings.DockOffset.X;
                case HorizontalAlignment.Center:
                    return monitorRect.Right/2 - ActualWidth/2 + _settings.DockOffset.X;
                case HorizontalAlignment.Right:
                    return monitorRect.Right - ActualWidth - _settings.DockOffset.X;
            }
            return double.NaN;
        }

        private double GetTop()
        {
            if (!_settings.IsDocked)
            {
                return _settings.Top;
            }
            var monitorRect = _settings.ScreenBounds;

            switch (_settings.VerticalAlignment)
            {
                case VerticalAlignment.Stretch:
                case VerticalAlignment.Top:
                    return monitorRect.Top + _settings.DockOffset.Y;
                case VerticalAlignment.Center:
                    return monitorRect.Bottom/2 - ActualHeight/2 + _settings.DockOffset.Y;
                case VerticalAlignment.Bottom:
                    return monitorRect.Bottom - ActualHeight - _settings.DockOffset.Y;
            }
            return double.NaN;
        }

        private double GetWidth()
        {
            return _settings.IsDocked && _settings.HorizontalAlignment == HorizontalAlignment.Stretch
                ? MaxWidth
                : _settings.Width;
        }

        private double GetHeight()
        {
            return _settings.IsDocked && _settings.VerticalAlignment == VerticalAlignment.Stretch
                ? MaxHeight
                : _settings.Height;
        }

        private void DismissWidgetExecute()
        {
            View?.Dismiss();
        }

        private void EditWidgetExecute()
        {
            Id.Edit(true);
        }

        private void ReloadWidgetExecute()
        {
            Id.Reload();
        }

        private void ToggleEnableWidgetExecute()
        {
            Id.ToggleEnable();
        }

        private void ManageAllWidgetsExecute()
        {
            new ManageWidgets().Show();
        }

        private void WidgetDockHorizontalExecute(HorizontalAlignment horizontalAlignment)
        {
            var previousAlignment = _settings.HorizontalAlignment;
            var previousIsDocked = _settings.IsDocked;
            _settings.HorizontalAlignment = horizontalAlignment;
            _settings.IsDocked = true;
            if (View != null)
            {
                View.UpdateUi(isDocked: previousIsDocked, dockHorizontalAlignment: previousAlignment);
                View.ShowIntro();
            }
        }

        private void WidgetDockVerticalExecute(VerticalAlignment verticalAlignment)
        {
            var previousAlignment = _settings.VerticalAlignment;
            var previousIsDocked = _settings.IsDocked;
            _settings.VerticalAlignment = verticalAlignment;
            _settings.IsDocked = true;
            if (View != null)
            {
                View.UpdateUi(isDocked: previousIsDocked, dockVerticalAlignment: previousAlignment);
                View.ShowIntro();
            }
        }

        private void WidgetUndockExecute()
        {
            var previousIsDocked = _settings.IsDocked;
            _settings.IsDocked = false;
            View?.UpdateUi(isDocked: previousIsDocked);
            View?.ShowIntro();
        }

        public void UpdatePosition()
        {
            Left = GetLeft();
            Top = GetTop();
        }

        public void UpdateSize()
        {
            Width = GetWidth();
            Height = GetHeight();
        }

        private void UpdateForceOnTopTimer()
        {
            if (!_settings.ForceOnTop || _settings.ForceOnTopInterval <= 0)
            {
                _onTopForceTimer?.Stop();
                return;
            }
            if (_onTopForceTimer == null)
            {
                _onTopForceTimer = new DispatcherTimer();
                _onTopForceTimer.Tick += (sender, args) => View?.ResetOnTop();
            }
            _onTopForceTimer.Interval = TimeSpan.FromMilliseconds(_settings.ForceOnTopInterval);
            _onTopForceTimer.Stop();
            _onTopForceTimer.Start();
        }

        public virtual void ReloadHotKeys()
        {
            if (_settings.HotKey != Key.None && _settings.OpenMode == OpenMode.Keyboard ||
                _settings.OpenMode == OpenMode.MouseAndKeyboard)
                HotkeyStore.RegisterHotkey(
                    new Hotkey(_settings.HotKey, _settings.HotKeyModifiers, _settings.FullscreenActivation, false,
                        _settings.ShowHotkeyIdentifier),
                    () =>
                        View?.ShowIntro(new IntroData(_settings.ShowHotkeyDuration, _settings.ToggleIntroOnHotkey,
                            _settings.ActivateOnShow, !_settings.StayOpenOnShowHotkey)));
            else
                HotkeyStore.RemoveHotkey(_settings.ShowHotkeyIdentifier);

            if (_settings.HideHotKey != Key.None)
                HotkeyStore.RegisterHotkey(
                    new Hotkey(_settings.HideHotKey, _settings.HideHotKeyModifiers, _settings.FullscreenActivation,
                        false, _settings.HideHotkeyIdentifier),
                    () => View?.HideUi(false));
            else
                HotkeyStore.RemoveHotkey(_settings.HideHotkeyIdentifier);
        }

        public virtual void DropExecute(DragEventArgs e)
        {
        }

        public virtual void MouseMoveExecute(MouseEventArgs e)
        {
            if (_settings.DetectIdle && _settings.UseMouseMoveIdleDetection)
                _settings.ActiveTimeEnd = DateTime.Now + _settings.IdleDuration;
        }

        public virtual void MouseDownExecute(MouseEventArgs e)
        {
            if (_settings.DetectIdle)
                _settings.ActiveTimeEnd = DateTime.Now + _settings.IdleDuration;
        }

        public virtual void MouseUpExecute(MouseEventArgs e)
        {
            if (_settings.DetectIdle)
                _settings.ActiveTimeEnd = DateTime.Now + _settings.IdleDuration;
        }

        public virtual void KeyDownExecute(KeyEventArgs e)
        {
            if (_settings.DetectIdle)
                _settings.ActiveTimeEnd = DateTime.Now + _settings.IdleDuration;
        }

        public virtual void KeyUpExecute(KeyEventArgs e)
        {
            if (_settings.DetectIdle)
                _settings.ActiveTimeEnd = DateTime.Now + _settings.IdleDuration;
        }

        public virtual void MouseDoubleClickExecute(MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                LeftMouseDoubleClickExecute(e);
        }

        public virtual void LeftMouseDoubleClickExecute(MouseButtonEventArgs e)
        {
        }

        public virtual void OnClose()
        {
            _onTopForceTimer?.Stop();
            _onTopForceTimer = null;
        }

        public virtual void OnIntro()
        {
        }

        public virtual void OnIntroEnd()
        {
        }

        public virtual void OnRefresh()
        {
        }

        public virtual void OnLoad()
        {
        }

        public virtual void OnUiLoad()
        {
        }

        public virtual void ReloadTimers()
        {
            UpdateForceOnTopTimer();
        }

        public virtual void OnDismiss()
        {
        }
    }
}