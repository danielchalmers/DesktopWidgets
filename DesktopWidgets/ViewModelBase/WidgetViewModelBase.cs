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

        private bool _isContextMenuOpen;

        private double _opacity;

        protected WidgetViewModelBase(WidgetId id)
        {
            Opacity = 0;
            _id = id;
            Settings = id.GetSettings();
            EditWidget = new RelayCommand(EditWidgetExecute);
            ReloadWidget = new RelayCommand(ReloadWidgetExecute);
            ToggleEnableWidget = new RelayCommand(ToggleEnableWidgetExecute);
            ManageAllWidgets = new RelayCommand(ManageAllWidgetsExecute);

            WidgetBringToFront = new RelayCommand<Window>(WidgetBringToFrontExecute);
            WidgetDockPosition = new RelayCommand<ScreenDockPosition>(WidgetDockPositionExecute);
            WidgetDockAlignment = new RelayCommand<ScreenDockAlignment>(WidgetDockAlignmentExecute);
        }

        public double Left
        {
            get { return GetLeft(); }
            set
            {
                if (Settings.Left != value)
                {
                    if (Settings.DockPosition == ScreenDockPosition.None)
                        Settings.Left = value;
                    RaisePropertyChanged(nameof(Left));
                }
            }
        }

        public double Top
        {
            get { return GetTop(); }
            set
            {
                if (Settings.Top != value)
                {
                    if (Settings.DockPosition == ScreenDockPosition.None)
                        Settings.Top = value;
                    RaisePropertyChanged(nameof(Top));
                }
            }
        }

        public double Width
        {
            get { return GetWidth(); }
        }

        public double Height
        {
            get { return GetHeight(); }
        }

        public double MaxWidth
        {
            get
            {
                return Settings.AutoMaxSize
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
                return Settings.AutoMaxSize
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

        private double GetLeft()
        {
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
                newLeft += Settings.DockOffset.X;
            }
            return newLeft;
        }

        private double GetTop()
        {
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
                newTop += Settings.DockOffset.Y;
            }
            return newTop;
        }

        private double GetWidth()
        {
            var newWidth = double.NaN;
            newWidth = Settings.Width;
            if (Settings.DockPosition == ScreenDockPosition.None)
            {
            }
            else
            {
                var horizontal = Settings.DockPosition.IsHorizontal();

                if (horizontal)
                {
                    switch (Settings.DockAlignment)
                    {
                        case ScreenDockAlignment.Top:
                            break;
                        default:
                        case ScreenDockAlignment.Center:
                            break;
                        case ScreenDockAlignment.Bottom:
                            break;
                        case ScreenDockAlignment.Stretch:
                            break;
                    }
                    newWidth = Settings.Width > Settings.MinWidth ? Settings.Width : Settings.MinWidth;
                }
                else
                {
                    switch (Settings.DockAlignment)
                    {
                        case ScreenDockAlignment.Top:
                            break;
                        default:
                        case ScreenDockAlignment.Center:
                            break;
                        case ScreenDockAlignment.Bottom:
                            break;
                        case ScreenDockAlignment.Stretch:
                            newWidth = MaxWidth;
                            break;
                    }
                }
            }
            return newWidth;
        }

        private double GetHeight()
        {
            var newHeight = double.NaN;
            newHeight = Settings.Height;
            if (Settings.DockPosition == ScreenDockPosition.None)
            {
            }
            else
            {
                var horizontal = Settings.DockPosition.IsHorizontal();

                if (horizontal)
                {
                    switch (Settings.DockAlignment)
                    {
                        case ScreenDockAlignment.Top:
                            break;
                        default:
                        case ScreenDockAlignment.Center:
                            break;
                        case ScreenDockAlignment.Bottom:
                            break;
                        case ScreenDockAlignment.Stretch:
                            newHeight = MaxHeight;
                            break;
                    }
                }
                else
                {
                    newHeight = Settings.Width > Settings.MinHeight ? Settings.Width : Settings.MinHeight;
                }
            }
            return newHeight;
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
            Settings.DockPosition = screenDockPosition;
            //UpdatePosition();
        }

        private void WidgetDockAlignmentExecute(ScreenDockAlignment screenDockAlignment)
        {
            Settings.DockAlignment = screenDockAlignment;
            //UpdatePosition();
        }
    }
}