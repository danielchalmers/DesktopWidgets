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

        private double _top;

        private double _width;

        protected WidgetViewModelBase(WidgetId id)
        {
            _id = id;
            Settings = id.GetSettings();
            EditWidget = new RelayCommand(EditWidgetExecute);
            ReloadWidget = new RelayCommand(ReloadWidgetExecute);
            ToggleEnableWidget = new RelayCommand(ToggleEnableWidgetExecute);
            ManageAllWidgets = new RelayCommand(ManageAllWidgetsExecute);

            WidgetDockPosition = new RelayCommand<ScreenDockPosition>(WidgetDockPositionExecute);
            WidgetDockAlignment = new RelayCommand<ScreenDockAlignment>(WidgetDockAlignmentExecute);
        }

        public double Left
        {
            get { return _left; }
            set
            {
                if (_left != value)
                {
                    if (Settings.DockPosition == ScreenDockPosition.None)
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
                    if (Settings.DockPosition == ScreenDockPosition.None)
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
                return double.IsNaN(Settings.MaxWidth)
                    ? MonitorHelper.GetMonitorBounds(Settings.Monitor).Width
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
                return double.IsNaN(Settings.MaxHeight)
                    ? MonitorHelper.GetMonitorBounds(Settings.Monitor).Height
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

        public ICommand EditWidget { get; private set; }
        public ICommand ReloadWidget { get; private set; }
        public ICommand ToggleEnableWidget { get; private set; }
        public ICommand ManageAllWidgets { get; private set; }

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

        private double GetLeft()
        {
            //if (double.IsNaN(ActualWidth) || ActualWidth < 1)
            //    return double.NaN;
            var newLeft = double.NaN;
            if (Settings.DockPosition == ScreenDockPosition.None)
            {
                newLeft = Settings.Left;
            }
            else
            {
                var horizontal = Settings.DockPosition.IsHorizontal();
                var monitorRect =
                    MonitorHelper.GetMonitorBounds(Settings.Monitor);

                if (horizontal)
                {
                    newLeft = Settings.DockPosition == ScreenDockPosition.Left
                        ? monitorRect.Left
                        : monitorRect.Right - ActualWidth;
                }
                else
                {
                    switch (Settings.DockAlignment)
                    {
                        case ScreenDockAlignment.Top:
                            newLeft = monitorRect.Left +
                                      (Settings.IgnoreCorners ? (Settings.CornerSize*2) : 0);
                            break;
                        default:
                        case ScreenDockAlignment.Center:
                            newLeft = monitorRect.Left + (monitorRect.Width/2) - (ActualWidth/2);
                            break;
                        case ScreenDockAlignment.Bottom:
                            newLeft = (monitorRect.Right - ActualWidth) -
                                      (Settings.IgnoreCorners ? (Settings.CornerSize*2) : 0);
                            break;
                        case ScreenDockAlignment.Stretch:
                            newLeft = monitorRect.Left;
                            break;
                    }
                }
                newLeft += Settings.DockPosition.ConvertHorizontalPadding(Settings.DockAlignment, Settings.DockOffset.X);
            }
            return newLeft;
        }

        private double GetTop()
        {
            //if (double.IsNaN(ActualHeight) || ActualHeight < 1)
            //    return double.NaN;
            var newTop = double.NaN;
            if (Settings.DockPosition == ScreenDockPosition.None)
            {
                newTop = Settings.Top;
            }
            else
            {
                var horizontal = Settings.DockPosition.IsHorizontal();
                var monitorRect =
                    MonitorHelper.GetMonitorBounds(Settings.Monitor);

                if (horizontal)
                {
                    switch (Settings.DockAlignment)
                    {
                        case ScreenDockAlignment.Top:
                            newTop = monitorRect.Top +
                                     (Settings.IgnoreCorners ? (Settings.CornerSize*2) : 0);
                            break;
                        default:
                        case ScreenDockAlignment.Center:
                            newTop = monitorRect.Top + (monitorRect.Height/2) - (ActualHeight/2);
                            break;
                        case ScreenDockAlignment.Bottom:
                            newTop = (monitorRect.Bottom - ActualHeight) -
                                     (Settings.IgnoreCorners ? (Settings.CornerSize*2) : 0);
                            break;
                        case ScreenDockAlignment.Stretch:
                            newTop = monitorRect.Top;
                            break;
                    }
                }
                else
                {
                    newTop = Settings.DockPosition == ScreenDockPosition.Top
                        ? monitorRect.Top
                        : monitorRect.Bottom - ActualHeight;
                }
                newTop += Settings.DockPosition.ConvertVerticalPadding(Settings.DockAlignment, Settings.DockOffset.Y);
            }
            return newTop;
        }

        private double GetWidth()
        {
            return Settings.DockPosition != ScreenDockPosition.None && Settings.DockPosition.IsVertical() &&
                   Settings.DockAlignment == ScreenDockAlignment.Stretch
                ? MaxWidth
                : Settings.Width;
        }

        private double GetHeight()
        {
            return Settings.DockPosition != ScreenDockPosition.None && Settings.DockPosition.IsHorizontal() &&
                   Settings.DockAlignment == ScreenDockAlignment.Stretch
                ? MaxHeight
                : Settings.Height;
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

        private void WidgetDockPositionExecute(ScreenDockPosition screenDockPosition)
        {
            var previousPosition = Settings.DockPosition;
            Settings.DockPosition = screenDockPosition;
            _id.GetView()?.UpdateUi(dockPosition: previousPosition);
        }

        private void WidgetDockAlignmentExecute(ScreenDockAlignment screenDockAlignment)
        {
            var previousAlignment = Settings.DockAlignment;
            Settings.DockAlignment = screenDockAlignment;
            _id.GetView()?.UpdateUi(dockAlignment: previousAlignment);
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
                HotkeyStore.RegisterHotkey(new Hotkey(Settings.HotKey, Settings.HotKeyModifiers, false),
                    () => _id.GetView()?.ShowIntro());
        }
    }
}