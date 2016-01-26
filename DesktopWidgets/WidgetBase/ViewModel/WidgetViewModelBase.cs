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
                if (Math.Abs(_left - value) > Properties.Settings.Default.DoubleComparisonTolerance)
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
                if (Math.Abs(_top - value) > Properties.Settings.Default.DoubleComparisonTolerance)
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
                if (Math.Abs(_width - value) > Properties.Settings.Default.DoubleComparisonTolerance)
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
                if (Math.Abs(_height - value) > Properties.Settings.Default.DoubleComparisonTolerance)
                {
                    _height = value;
                    RaisePropertyChanged(nameof(Height));
                }
            }
        }

        public double MaxWidth
        {
            get
            {
                return double.IsNaN(_settings.MaxWidth) && _settings.MaxSizeUseScreen
                    ? _settings.ScreenBounds.Width
                    : _settings.MaxWidth;
            }
            set
            {
                if (Math.Abs(_settings.MaxWidth - value) > Properties.Settings.Default.DoubleComparisonTolerance)
                {
                    _settings.MaxWidth = value;
                    RaisePropertyChanged(nameof(MaxWidth));
                }
            }
        }

        public double MaxHeight
        {
            get
            {
                return double.IsNaN(_settings.MaxHeight) && _settings.MaxSizeUseScreen
                    ? _settings.ScreenBounds.Height
                    : _settings.MaxHeight;
            }
            set
            {
                if (Math.Abs(_settings.MaxHeight - value) > Properties.Settings.Default.DoubleComparisonTolerance)
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
                if (Math.Abs(_actualWidth - value) > Properties.Settings.Default.DoubleComparisonTolerance &&
                    !double.IsNaN(value) && value > 0)
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
                if (Math.Abs(_actualHeight - value) > Properties.Settings.Default.DoubleComparisonTolerance &&
                    !double.IsNaN(value) && value > 0)
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
            View?.HideUi();
        }

        private void EditWidgetExecute()
        {
            Id.Edit();
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
                if (_settings.AutoDetectScreenBounds)
                    _settings.ScreenBounds = ScreenHelper.GetScreen(View).ToRect(_settings.IgnoreAppBars);
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
                if (_settings.AutoDetectScreenBounds)
                    _settings.ScreenBounds = ScreenHelper.GetScreen(View).ToRect(_settings.IgnoreAppBars);
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
            if (_settings.OpenMode == OpenMode.Keyboard || _settings.OpenMode == OpenMode.MouseAndKeyboard)
                HotkeyStore.RegisterHotkey(_settings.Identifier.Guid,
                    new Hotkey(_settings.HotKey, _settings.HotKeyModifiers, _settings.FullscreenActivation),
                    () =>
                        View?.ShowIntro(activate: _settings.ActivateOnShow, reversable: _settings.ToggleIntroOnHotkey));
            else
                HotkeyStore.RemoveHotkey(Id.Guid);
        }

        public virtual void OnClose()
        {
            _onTopForceTimer?.Stop();
            _onTopForceTimer = null;
        }

        public virtual void DropExecute(DragEventArgs e)
        {
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
    }
}