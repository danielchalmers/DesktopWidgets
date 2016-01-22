using System;
using System.Windows;
using System.Windows.Input;
using DesktopWidgets.Classes;
using DesktopWidgets.Helpers;
using DesktopWidgets.Windows;
using GalaSoft.MvvmLight.Command;

namespace DesktopWidgets.ViewModelBase
{
    public abstract class WidgetViewModelBase : GalaSoft.MvvmLight.ViewModelBase
    {
        public readonly WidgetId _id;
        private readonly WidgetSettingsBase Settings;
        private double _actualHeight;
        private double _actualWidth;

        private double _height;

        private bool _isContextMenuOpen;

        private double _left;

        private bool _onTop;

        private double _top;

        private double _width;
        public Action RefreshAction;

        protected WidgetViewModelBase(WidgetId id)
        {
            _id = id;
            Settings = id.GetSettings();
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
                if (_left != value)
                {
                    if (!Settings.IsDocked)
                        Settings.Left = value;
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
                if (_top != value)
                {
                    if (!Settings.IsDocked)
                        Settings.Top = value;
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
            get
            {
                return double.IsNaN(Settings.MaxWidth) && Settings.MaxSizeUseScreen
                    ? Settings.ScreenBounds.Width
                    : Settings.MaxWidth;
            }
            set
            {
                if (Settings.MaxWidth != value)
                {
                    Settings.MaxWidth = value;
                    RaisePropertyChanged(nameof(MaxWidth));
                }
            }
        }

        public double MaxHeight
        {
            get
            {
                return double.IsNaN(Settings.MaxHeight) && Settings.MaxSizeUseScreen
                    ? Settings.ScreenBounds.Height
                    : Settings.MaxHeight;
            }
            set
            {
                if (Settings.MaxHeight != value)
                {
                    Settings.MaxHeight = value;
                    RaisePropertyChanged(nameof(MaxHeight));
                }
            }
        }

        public double ActualWidth
        {
            get { return _actualWidth; }
            set
            {
                if (_actualWidth != value && !double.IsNaN(value) && value > 0)
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
                if (_actualHeight != value && !double.IsNaN(value) && value > 0)
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
            if (!Settings.IsDocked)
            {
                return Settings.Left;
            }
            var monitorRect = Settings.ScreenBounds;

            switch (Settings.HorizontalAlignment)
            {
                case HorizontalAlignment.Stretch:
                case HorizontalAlignment.Left:
                    return monitorRect.Left + Settings.DockOffset.X;
                case HorizontalAlignment.Center:
                    return monitorRect.Right/2 - ActualWidth/2 + Settings.DockOffset.X;
                case HorizontalAlignment.Right:
                    return monitorRect.Right - ActualWidth - Settings.DockOffset.X;
            }
            return double.NaN;
        }

        private double GetTop()
        {
            if (!Settings.IsDocked)
            {
                return Settings.Top;
            }
            var monitorRect = Settings.ScreenBounds;

            switch (Settings.VerticalAlignment)
            {
                case VerticalAlignment.Stretch:
                case VerticalAlignment.Top:
                    return monitorRect.Top + Settings.DockOffset.Y;
                case VerticalAlignment.Center:
                    return monitorRect.Bottom/2 - ActualHeight/2 + Settings.DockOffset.Y;
                case VerticalAlignment.Bottom:
                    return monitorRect.Bottom - ActualHeight - Settings.DockOffset.Y;
            }
            return double.NaN;
        }

        private double GetWidth()
        {
            return Settings.IsDocked && Settings.HorizontalAlignment == HorizontalAlignment.Stretch
                ? MaxWidth
                : Settings.Width;
        }

        private double GetHeight()
        {
            return Settings.IsDocked && Settings.VerticalAlignment == VerticalAlignment.Stretch
                ? MaxHeight
                : Settings.Height;
        }

        private void DismissWidgetExecute()
        {
            _id.GetView()?.HideUI();
        }

        private void EditWidgetExecute()
        {
            _id.Edit();
        }

        private void ReloadWidgetExecute()
        {
            _id.Reload();
        }

        private void ToggleEnableWidgetExecute()
        {
            _id.ToggleEnable();
        }

        private void ManageAllWidgetsExecute()
        {
            new ManageWidgets().Show();
        }

        private void WidgetDockHorizontalExecute(HorizontalAlignment horizontalAlignment)
        {
            var previousAlignment = Settings.HorizontalAlignment;
            var previousIsDocked = Settings.IsDocked;
            var view = _id.GetView();
            Settings.HorizontalAlignment = horizontalAlignment;
            Settings.IsDocked = true;
            if (view != null)
            {
                view.UpdateUi(isDocked: previousIsDocked, dockHorizontalAlignment: previousAlignment);
                view.ShowIntro(reversable: false);
            }
        }

        private void WidgetDockVerticalExecute(VerticalAlignment verticalAlignment)
        {
            var previousAlignment = Settings.VerticalAlignment;
            var previousIsDocked = Settings.IsDocked;
            var view = _id.GetView();
            Settings.VerticalAlignment = verticalAlignment;
            Settings.IsDocked = true;
            if (view != null)
            {
                view.UpdateUi(isDocked: previousIsDocked, dockVerticalAlignment: previousAlignment);
                view.ShowIntro(reversable: false);
            }
        }

        private void WidgetUndockExecute()
        {
            var previousIsDocked = Settings.IsDocked;
            Settings.IsDocked = false;
            _id.GetView()?.UpdateUi(isDocked: previousIsDocked);
            _id.GetView()?.ShowIntro(reversable: false);
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

        public virtual void ReloadHotKeys()
        {
            if (Settings.OpenMode == OpenMode.Keyboard || Settings.OpenMode == OpenMode.MouseAndKeyboard)
                HotkeyStore.RegisterHotkey(Settings.Identifier.Guid,
                    new Hotkey(Settings.HotKey, Settings.HotKeyModifiers, Settings.FullscreenActivation),
                    () =>
                        _id.GetView()?
                            .ShowIntro(activate: Settings.ActivateOnShow, reversable: Settings.ToggleIntroOnHotkey));
            else
                HotkeyStore.RemoveHotkey(_id.Guid);
        }

        public virtual void OnClose()
        {
        }

        public virtual void DropExecute(DragEventArgs e)
        {
        }

        public virtual void OnIntroFinish()
        {
        }
    }
}