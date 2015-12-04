using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using DesktopWidgets.Classes;
using DesktopWidgets.Commands;
using DesktopWidgets.Helpers;
using DesktopWidgets.Properties;
using NHotkey;
using NHotkey.Wpf;

namespace DesktopWidgets.ViewModelBase
{
    public abstract class WidgetViewModelBase : ViewModelBase
    {
        private readonly MouseChecker _mouseChecker;
        private readonly DispatcherTimer _onTopForceTimer;
        private readonly WidgetSettings _settings;
        private double _actualHeight;
        private double _actualWidth;

        private double _height;

        private double _left;

        private bool _onTop;


        private double _opacity;

        private double _top;

        private double _width;

        public bool QueueIntro;

        protected WidgetViewModelBase(WidgetId id)
        {
            Opacity = 0;
            MouseDownCommand = new DelegateCommand(MouseDown);
            LocationChangedCommand = new DelegateCommand(LocationChanged);
            ClosingCommand = new DelegateCommand(OnClosing);
            _settings = id.GetSettings();
            if (_settings.ForceOnTop)
            {
                _onTopForceTimer = new DispatcherTimer();
                _onTopForceTimer.Interval = TimeSpan.FromMilliseconds(100);
                _onTopForceTimer.Tick += delegate
                {
                    OnTop = false;
                    OnTop = true;
                };
                _onTopForceTimer.Start();
            }
            _mouseChecker = new MouseChecker(id, this);
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

        public ICommand ClosingCommand { get; private set; }
        public ICommand MouseDownCommand { get; private set; }
        public ICommand LocationChangedCommand { get; private set; }

        private void ReloadHotKeys()
        {
            try
            {
                if (_settings.OpenMode == OpenMode.Keyboard || _settings.OpenMode == OpenMode.MouseAndKeyboard)
                {
                    HotkeyManager.Current.Remove("Show");
                    HotkeyManager.Current.AddOrReplace("Show", _settings.HotKey, _settings.HotKeyModifiers, OnHotKey);
                }
            }
            catch (HotkeyAlreadyRegisteredException)
            {
            }
        }

        private void OnHotKey(object sender, HotkeyEventArgs e)
        {
            ShowIntro();
            e.Handled = true;
        }

        public void ShowIntro()
        {
            if (_settings.OpenMode == OpenMode.AlwaysOpen)
                return;

            if (Opacity < 1)
            {
                QueueIntro = true;
                return;
            }
            QueueIntro = false;

            _mouseChecker.KeepOpen = true;
            _mouseChecker.Show();
            DelayedAction.RunAction(Settings.Default.IntroDuration, () =>
            {
                _mouseChecker.KeepOpen = false;
                _mouseChecker.Hide(checkHideStatus: true);
            });
        }

        public void ShowUI()
        {
            _mouseChecker.Show();
        }

        public void HideUI()
        {
            _mouseChecker.Hide();
        }

        public void UpdateUi()
        {
            UpdatePosition();
            ReloadHotKeys();
            _mouseChecker.UpdateIntervals();
            _mouseChecker.Stop();
            _mouseChecker.Start();
        }

        private void UpdatePosition()
        {
            Width = _settings.Width;
            Height = _settings.Height;
            if (_settings.DockPosition == ScreenDockPosition.None)
            {
                Left = _settings.Left;
                Top = _settings.Top;
            }
            else
            {
                var horizontal = _settings.DockPosition.IsHorizontal();
                var monitorRect =
                    MonitorHelper.GetMonitorBounds(_settings.Monitor);

                if (horizontal)
                {
                    double newTop;
                    switch (_settings.DockAlignment)
                    {
                        case ScreenDockAlignment.Top:
                            newTop = monitorRect.Top +
                                     (_settings.IgnoreCorners ? (Settings.Default.CornerSize*2) : 0);
                            break;
                        default:
                        case ScreenDockAlignment.Center:
                            newTop = monitorRect.Top + (monitorRect.Height/2) - (ActualHeight/2);
                            break;
                        case ScreenDockAlignment.Bottom:
                            newTop = (monitorRect.Bottom - ActualHeight) -
                                     (_settings.IgnoreCorners ? (Settings.Default.CornerSize*2) : 0);
                            break;
                        case ScreenDockAlignment.Stretch:
                            Height = monitorRect.Height;
                            newTop = monitorRect.Top;
                            break;
                    }
                    Top = newTop;
                    Width = _settings.Width > _settings.MinWidth ? _settings.Width : _settings.MinWidth;
                    Left = _settings.DockPosition == ScreenDockPosition.Left
                        ? monitorRect.Left
                        : monitorRect.Right - ActualWidth;
                }
                else
                {
                    double newLeft;
                    switch (_settings.DockAlignment)
                    {
                        case ScreenDockAlignment.Top:
                            newLeft = monitorRect.Left +
                                      (_settings.IgnoreCorners ? (Settings.Default.CornerSize*2) : 0);
                            break;
                        default:
                        case ScreenDockAlignment.Center:
                            newLeft = monitorRect.Left + (monitorRect.Width/2) - (ActualWidth/2);
                            break;
                        case ScreenDockAlignment.Bottom:
                            newLeft = (monitorRect.Right - ActualWidth) -
                                      (_settings.IgnoreCorners ? (Settings.Default.CornerSize*2) : 0);
                            break;
                        case ScreenDockAlignment.Stretch:
                            Width = monitorRect.Width;
                            newLeft = monitorRect.Left;
                            break;
                    }
                    Left = newLeft;
                    Height = _settings.Width > _settings.MinHeight ? _settings.Width : _settings.MinHeight;
                    Top = _settings.DockPosition == ScreenDockPosition.Top
                        ? monitorRect.Top
                        : monitorRect.Bottom - ActualHeight;
                }
            }
        }

        private void OnClosing(object parameter)
        {
            _mouseChecker.Stop();
        }

        private void MouseDown(object parameter)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed && _settings.DockPosition == ScreenDockPosition.None)
                (parameter as Window).DragMove();
        }

        private void LocationChanged(object parameter)
        {
            if (_settings.SnapToScreenEdges)
                (parameter as Window).SnapToScreenEdges();
        }
    }
}