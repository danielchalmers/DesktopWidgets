using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using DesktopWidgets.Events;
using DesktopWidgets.Helpers;
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
        private DispatcherTimer _actionBarHideTimer;
        private double _actualHeight;
        private double _actualWidth;
        private bool _isContextMenuOpen;
        private bool _keepActionBarOpen;
        private DispatcherTimer _onTopForceTimer;

        protected WidgetViewModelBase(WidgetId id)
        {
            Id = id;
            _settings = id.GetSettings();
            DismissWidget = new RelayCommand(DismissWidgetExecute);
            EditWidget = new RelayCommand(EditWidgetExecute);
            ReloadWidget = new RelayCommand(ReloadWidgetExecute);
            ToggleEnableWidget = new RelayCommand(ToggleEnableWidgetExecute);
            MuteWidget = new RelayCommand(MuteWidgetExecute);
            MuteUnmuteWidget = new RelayCommand(MuteUnmuteWidgetExecute);
            ManageAllWidgets = new RelayCommand(ManageAllWidgetsExecute);

            Drop = new RelayCommand<DragEventArgs>(DropExecute);
            MouseMove = new RelayCommand<MouseEventArgs>(MouseMoveExecute);
            MouseDown = new RelayCommand<MouseButtonEventArgs>(MouseDownExecute);
            MouseUp = new RelayCommand<MouseButtonEventArgs>(MouseUpExecute);
            KeyDown = new RelayCommand<KeyEventArgs>(KeyDownExecute);
            KeyUp = new RelayCommand<KeyEventArgs>(KeyUpExecute);
            PreviewKeyDown = new RelayCommand<KeyEventArgs>(PreviewKeyDownExecute);
            PreviewKeyUp = new RelayCommand<KeyEventArgs>(PreviewKeyUpExecute);
            MouseDoubleClick = new RelayCommand<MouseButtonEventArgs>(MouseDoubleClickExecute);

            WidgetDockHorizontal = new RelayCommand<HorizontalAlignment>(WidgetDockHorizontalExecute);
            WidgetDockVertical = new RelayCommand<VerticalAlignment>(WidgetDockVerticalExecute);
            WidgetUndock = new RelayCommand(WidgetUndockExecute);
        }

        public bool AllowDrop { get; set; }

        public WidgetView View { get; set; }

        public double Left
        {
            get { return _settings.Left; }
            set
            {
                if (!_settings.IsDocked)
                {
                    _settings.Left = value;
                }
                _settings.Left = value;
                RaisePropertyChanged();
            }
        }

        public double Top
        {
            get { return _settings.Top; }
            set
            {
                if (!_settings.IsDocked)
                {
                    _settings.Top = value;
                }
                _settings.Top = value;
                RaisePropertyChanged();
            }
        }

        public double Width
        {
            get { return _settings.Style.Width; }
            set
            {
                _settings.Style.Width = value;
                RaisePropertyChanged();
            }
        }

        public double Height
        {
            get { return _settings.Style.Height; }
            set
            {
                _settings.Style.Height = value;
                RaisePropertyChanged();
            }
        }

        public double MaxWidth => double.IsNaN(_settings.Style.MaxWidth) && _settings.MaxSizeUseScreen
            ? GetScreenBounds().Width
            : _settings.Style.MaxWidth;

        public double MaxHeight => double.IsNaN(_settings.Style.MaxHeight) && _settings.MaxSizeUseScreen
            ? GetScreenBounds().Height
            : _settings.Style.MaxHeight;

        public double ActualWidth
        {
            get { return _actualWidth; }
            set
            {
                if (value > 0 && value.IsEqual(_actualWidth))
                {
                    _actualWidth = value;
                    RaisePropertyChanged();
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
                    RaisePropertyChanged();
                }
            }
        }

        public ICommand DismissWidget { get; private set; }
        public ICommand EditWidget { get; private set; }
        public ICommand ReloadWidget { get; private set; }
        public ICommand ToggleEnableWidget { get; private set; }
        public ICommand MuteWidget { get; private set; }
        public ICommand MuteUnmuteWidget { get; private set; }
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
        public ICommand PreviewKeyDown { get; set; }
        public ICommand PreviewKeyUp { get; set; }
        public ICommand MouseDoubleClick { get; set; }

        public bool IsContextMenuOpen
        {
            get { return _isContextMenuOpen; }
            set
            {
                if (_isContextMenuOpen != value)
                {
                    _isContextMenuOpen = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool KeepActionBarOpen
        {
            get { return _keepActionBarOpen; }
            set
            {
                if (_keepActionBarOpen != value)
                {
                    _keepActionBarOpen = value;
                    RaisePropertyChanged();
                }
            }
        }

        private double GetLeft()
        {
            if (!_settings.IsDocked)
            {
                return _settings.Left;
            }
            var monitorRect = GetScreenBounds();

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
            var monitorRect = GetScreenBounds();

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
                : _settings.Style.Width;
        }

        private double GetHeight()
        {
            return _settings.IsDocked && _settings.VerticalAlignment == VerticalAlignment.Stretch
                ? MaxHeight
                : _settings.Style.Height;
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

        public void SetScreenBounds()
        {
            if (!_settings.AutoDetectScreenBounds || View == null)
            {
                return;
            }
            _settings.ScreenBounds = GetActualScreenBounds();
        }

        public Rect GetScreenBounds()
        {
            return _settings.ScreenBounds == Rect.Empty ? SystemParameters.WorkArea : _settings.ScreenBounds;
        }

        public Rect GetActualScreenBounds()
        {
            if (!_settings.AutoDetectScreenBounds || View == null)
            {
                return GetScreenBounds();
            }
            var screen = ScreenHelper.GetScreen(View);
            return screen.Primary ? Rect.Empty : screen.ToRect(_settings.IgnoreAppBars);
        }

        private void WidgetDockHorizontalExecute(HorizontalAlignment horizontalAlignment)
        {
            var previousAlignment = _settings.HorizontalAlignment;
            var previousIsDocked = _settings.IsDocked;
            _settings.HorizontalAlignment = horizontalAlignment;
            if (horizontalAlignment == HorizontalAlignment.Left && _settings.ActionBarStyle.Dock == Dock.Left)
            {
                _settings.ActionBarStyle.Dock = Dock.Right;
            }
            if (horizontalAlignment == HorizontalAlignment.Right && _settings.ActionBarStyle.Dock == Dock.Right)
            {
                _settings.ActionBarStyle.Dock = Dock.Left;
            }
            _settings.IsDocked = true;
            if (View != null)
            {
                SetScreenBounds();
                View?.UpdateUi(isDocked: previousIsDocked, dockHorizontalAlignment: previousAlignment);
                View?.ShowIntro();
            }
        }

        private void WidgetDockVerticalExecute(VerticalAlignment verticalAlignment)
        {
            var previousAlignment = _settings.VerticalAlignment;
            var previousIsDocked = _settings.IsDocked;
            _settings.VerticalAlignment = verticalAlignment;
            if (verticalAlignment == VerticalAlignment.Top && _settings.ActionBarStyle.Dock == Dock.Top)
            {
                _settings.ActionBarStyle.Dock = Dock.Bottom;
            }
            if (verticalAlignment == VerticalAlignment.Bottom && _settings.ActionBarStyle.Dock == Dock.Bottom)
            {
                _settings.ActionBarStyle.Dock = Dock.Top;
            }
            _settings.IsDocked = true;
            if (View != null)
            {
                SetScreenBounds();
                View?.UpdateUi(isDocked: previousIsDocked, dockVerticalAlignment: previousAlignment);
                View?.ShowIntro();
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
            if (!_settings.ForceTopmost || _settings.ForceOnTopInterval <= 0)
            {
                _onTopForceTimer?.Stop();
                _onTopForceTimer = null;
                return;
            }
            if (_onTopForceTimer == null)
            {
                _onTopForceTimer = new DispatcherTimer();
                _onTopForceTimer.Tick +=
                    (sender, args) =>
                    {
                        if (App.WidgetViews.Where(x => x.Id != Id).All(x => !x.IsMouseOver))
                        {
                            View?.ThisApp?.BringToFront();
                        }
                    };
            }
            _onTopForceTimer.Interval = TimeSpan.FromMilliseconds(_settings.ForceOnTopInterval);
            _onTopForceTimer.Stop();
            _onTopForceTimer.Start();
        }

        private void UpdateActionBarTimer()
        {
            if (_settings.ActionBarStyle.StayOpenDuration <= 0)
            {
                _actionBarHideTimer?.Stop();
                return;
            }
            if (_actionBarHideTimer == null)
            {
                _actionBarHideTimer = new DispatcherTimer();
                _actionBarHideTimer.Tick += (sender, args) =>
                {
                    KeepActionBarOpen = false;
                    _actionBarHideTimer.Stop();
                };
            }
            _actionBarHideTimer.Interval = TimeSpan.FromMilliseconds(_settings.ActionBarStyle.StayOpenDuration);
            _actionBarHideTimer.Stop();
            _actionBarHideTimer.Start();
        }

        private void StartActionBarKeepOpen()
        {
            if (_actionBarHideTimer == null)
            {
                return;
            }
            _actionBarHideTimer.Stop();
            _actionBarHideTimer.Start();
            KeepActionBarOpen = true;
        }

        public virtual void DropExecute(DragEventArgs e)
        {
        }

        public virtual void MouseMoveExecute(MouseEventArgs e)
        {
            if (_settings.DetectIdle && _settings.UseMouseMoveIdleDetection)
            {
                _settings.ActiveTimeEnd = DateTime.Now + _settings.IdleDuration;
            }

            StartActionBarKeepOpen();
        }

        public virtual void MouseDownExecute(MouseButtonEventArgs e)
        {
            if (_settings.DetectIdle)
            {
                _settings.ActiveTimeEnd = DateTime.Now + _settings.IdleDuration;
            }

            foreach (var eventPair in App.WidgetsSettingsStore.EventActionPairs)
            {
                var evnt = eventPair.Event as WidgetMouseDownEvent;
                if (evnt == null || eventPair.Disabled || evnt.WidgetId?.Guid != Id?.Guid)
                {
                    continue;
                }
                if (evnt.MouseButton == e.ChangedButton)
                {
                    eventPair.Action.Execute();
                }
            }
        }

        public virtual void MouseUpExecute(MouseButtonEventArgs e)
        {
            if (_settings.DetectIdle)
            {
                _settings.ActiveTimeEnd = DateTime.Now + _settings.IdleDuration;
            }

            foreach (var eventPair in App.WidgetsSettingsStore.EventActionPairs)
            {
                var evnt = eventPair.Event as WidgetMouseUpEvent;
                if (evnt == null || eventPair.Disabled || evnt.WidgetId?.Guid != Id?.Guid)
                {
                    continue;
                }
                if (evnt.MouseButton == e.ChangedButton)
                {
                    eventPair.Action.Execute();
                }
            }
        }

        public virtual void KeyDownExecute(KeyEventArgs e)
        {
            if (_settings.DetectIdle)
            {
                _settings.ActiveTimeEnd = DateTime.Now + _settings.IdleDuration;
            }
        }

        public virtual void KeyUpExecute(KeyEventArgs e)
        {
            if (_settings.DetectIdle)
            {
                _settings.ActiveTimeEnd = DateTime.Now + _settings.IdleDuration;
            }
        }

        public virtual void PreviewKeyDownExecute(KeyEventArgs e)
        {
        }

        public virtual void PreviewKeyUpExecute(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    DismissWidgetExecute();
                    e.Handled = true;
                    break;
                case Key.F5:
                    ReloadWidgetExecute();
                    e.Handled = true;
                    break;
                case Key.Apps:
                    IsContextMenuOpen = true;
                    e.Handled = true;
                    break;
            }
        }

        public virtual void MouseDoubleClickExecute(MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                LeftMouseDoubleClickExecute(e);
            }

            foreach (var eventPair in App.WidgetsSettingsStore.EventActionPairs)
            {
                var evnt = eventPair.Event as WidgetMouseDoubleClickEvent;
                if (evnt == null || eventPair.Disabled || evnt.WidgetId?.Guid != Id?.Guid)
                {
                    continue;
                }
                if (evnt.MouseButton == e.ChangedButton)
                {
                    eventPair.Action.Execute();
                }
            }
        }

        public virtual void LeftMouseDoubleClickExecute(MouseButtonEventArgs e)
        {
        }

        public virtual void OnClose()
        {
            _onTopForceTimer?.Stop();
            _onTopForceTimer = null;
            _actionBarHideTimer?.Stop();
            _actionBarHideTimer = null;
        }

        public virtual void OnIntro()
        {
            foreach (var eventPair in App.WidgetsSettingsStore.EventActionPairs)
            {
                var evnt = eventPair.Event as WidgetIntroEvent;
                if (evnt == null || eventPair.Disabled || evnt.WidgetId?.Guid != Id?.Guid)
                {
                    continue;
                }
                eventPair.Action.Execute();
            }
        }

        public virtual void OnIntroEnd()
        {
            foreach (var eventPair in App.WidgetsSettingsStore.EventActionPairs)
            {
                var evnt = eventPair.Event as WidgetIntroEndEvent;
                if (evnt == null || eventPair.Disabled || evnt.WidgetId?.Guid != Id?.Guid)
                {
                    continue;
                }
                eventPair.Action.Execute();
            }
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
            UpdateActionBarTimer();
        }

        public virtual void OnDismiss()
        {
            foreach (var eventPair in App.WidgetsSettingsStore.EventActionPairs)
            {
                var evnt = eventPair.Event as WidgetDismissEvent;
                if (evnt == null || eventPair.Disabled || evnt.WidgetId?.Guid != Id?.Guid)
                {
                    continue;
                }
                eventPair.Action.Execute();
            }
        }

        public virtual void OnSpecialEvent()
        {
            if (!_settings.IsMuted())
            {
                foreach (var eventPair in App.WidgetsSettingsStore.EventActionPairs)
                {
                    var evnt = eventPair.Event as WidgetSpecialEvent;
                    if (evnt == null || eventPair.Disabled || evnt.WidgetId?.Guid != Id?.Guid)
                    {
                        continue;
                    }
                    eventPair.Action.Execute();
                }
            }
        }

        public virtual void OnHide()
        {
            foreach (var eventPair in App.WidgetsSettingsStore.EventActionPairs)
            {
                var evnt = eventPair.Event as WidgetHideEvent;
                if (evnt == null || eventPair.Disabled || evnt.WidgetId?.Guid != Id?.Guid)
                {
                    continue;
                }
                eventPair.Action.Execute();
            }
        }

        public virtual void OnShow()
        {
            foreach (var eventPair in App.WidgetsSettingsStore.EventActionPairs)
            {
                var evnt = eventPair.Event as WidgetShowEvent;
                if (evnt == null || eventPair.Disabled || evnt.WidgetId?.Guid != Id?.Guid)
                {
                    continue;
                }
                eventPair.Action.Execute();
            }
        }

        public virtual void ExecuteSpecialAction()
        {
        }

        private void MuteWidgetExecute()
        {
            Id.Mute(Properties.Settings.Default.MuteDuration);
        }

        private void MuteUnmuteWidgetExecute()
        {
            Id.ToggleMute(Properties.Settings.Default.MuteDuration);
        }
    }
}