#region

using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using DesktopWidgets.Helpers;
using DesktopWidgets.Properties;
using DesktopWidgets.View;

#endregion

namespace DesktopWidgets.Classes
{
    public class MouseChecker
    {
        private readonly DispatcherTimer _hideTimer;
        private readonly DispatcherTimer _mouseCheckTimer;
        private readonly WidgetSettingsBase _settings;
        private readonly DispatcherTimer _showTimer;
        private readonly WidgetView _view;
        public bool KeepOpenForIntro;

        public MouseChecker(WidgetView view, WidgetSettingsBase settings)
        {
            _view = view;
            _settings = settings;
            // Setup mouse checker, hide, show timers
            _mouseCheckTimer = new DispatcherTimer();
            _hideTimer = new DispatcherTimer();
            _showTimer = new DispatcherTimer();

            _mouseCheckTimer.Tick += (sender, args) => Update();
            _hideTimer.Tick += delegate
            {
                Hide();
                _hideTimer.Stop();
            };
            _showTimer.Tick += delegate
            {
                Show();
                _showTimer.Stop();
            };

            UpdateIntervals();
        }

        private Point GetMouseLocation()
            => new Point(Control.MousePosition.X, Control.MousePosition.Y);

        public void UpdateIntervals()
        {
            _mouseCheckTimer.Interval = TimeSpan.FromMilliseconds(Settings.Default.MouseBoundsPollingInterval);
            _hideTimer.Interval = TimeSpan.FromMilliseconds(_settings.HideDelay);
            _showTimer.Interval = TimeSpan.FromMilliseconds(_settings.ShowDelay);
        }

        public void Start()
        {
            _mouseCheckTimer.Start();
        }

        public void Stop()
        {
            _mouseCheckTimer.Stop();
        }

        private bool IsMouseInMouseBounds()
        {
            // Return is mouse in correct bounds to show sidebar.
            Rect checkBounds;
            if (_settings.CustomMouseDetectionBounds.Width > 0 && _settings.CustomMouseDetectionBounds.Height > 0)
                checkBounds = _settings.CustomMouseDetectionBounds;
            else if (_settings.DockPosition == ScreenDockPosition.None)
            {
                checkBounds = _view.GetBounds();
                checkBounds = new Rect(checkBounds.Left - _settings.MouseBounds, checkBounds.Top - _settings.MouseBounds,
                    checkBounds.Width + (_settings.MouseBounds*2), checkBounds.Height + (_settings.MouseBounds*2));
            }
            else
            {
                if (_settings.StretchBounds)
                {
                    checkBounds = MonitorHelper.GetMonitorBounds(_settings.Monitor);
                    switch (_settings.DockPosition)
                    {
                        case ScreenDockPosition.Left:
                            checkBounds = new Rect(checkBounds.Left, checkBounds.Top, _settings.MouseBounds,
                                checkBounds.Height);
                            break;
                        case ScreenDockPosition.Right:
                            checkBounds = new Rect((checkBounds.Left + _view.ActualWidth) - _settings.MouseBounds,
                                checkBounds.Top, _settings.MouseBounds, checkBounds.Height);
                            break;
                        case ScreenDockPosition.Top:
                            checkBounds = new Rect(checkBounds.Left, checkBounds.Top, checkBounds.Width,
                                _settings.MouseBounds);
                            break;
                        case ScreenDockPosition.Bottom:
                            checkBounds = new Rect(checkBounds.Left,
                                (checkBounds.Top + _view.ActualHeight) - _settings.MouseBounds, checkBounds.Width,
                                _settings.MouseBounds);
                            break;
                    }
                }
                else
                {
                    checkBounds = _view.GetBounds();
                    switch (_settings.DockPosition)
                    {
                        case ScreenDockPosition.Left:
                            checkBounds = new Rect(checkBounds.Left, checkBounds.Top, _settings.MouseBounds,
                                checkBounds.Height);
                            break;
                        case ScreenDockPosition.Right:
                            checkBounds = new Rect((checkBounds.Left + _view.ActualWidth) - _settings.MouseBounds,
                                checkBounds.Top, _settings.MouseBounds, checkBounds.Height);
                            break;
                        case ScreenDockPosition.Top:
                            checkBounds = new Rect(checkBounds.Left, checkBounds.Top, checkBounds.Width,
                                _settings.MouseBounds);
                            break;
                        case ScreenDockPosition.Bottom:
                            checkBounds = new Rect(checkBounds.Left,
                                (checkBounds.Top + _view.ActualHeight) - _settings.MouseBounds, checkBounds.Width,
                                _settings.MouseBounds);
                            break;
                    }
                }
            }

            return checkBounds.Contains(GetMouseLocation());
        }

        private bool IsMouseInWindowBounds()
        {
            // Return is mouse in correct bounds to show sidebar.
            Rect checkBounds;
            if (_settings.StretchBounds)
            {
                checkBounds = MonitorHelper.GetMonitorBounds(_settings.Monitor);
                switch (_settings.DockPosition)
                {
                    default:
                    case ScreenDockPosition.Left:
                        checkBounds = new Rect(checkBounds.Left, checkBounds.Top, _view.ActualWidth,
                            checkBounds.Height);
                        break;
                    case ScreenDockPosition.Right:
                        checkBounds = new Rect(checkBounds.Right - _view.ActualWidth, checkBounds.Top,
                            _view.ActualWidth, checkBounds.Height);
                        break;
                    case ScreenDockPosition.Top:
                        checkBounds = new Rect(checkBounds.Left, checkBounds.Top, checkBounds.Width,
                            _view.ActualHeight);
                        break;
                    case ScreenDockPosition.Bottom:
                        checkBounds = new Rect(checkBounds.Left, checkBounds.Bottom - _view.ActualHeight,
                            checkBounds.Width, _view.ActualHeight);
                        break;
                }
            }
            else
            {
                checkBounds = _view.GetBounds();
            }

            return checkBounds.Contains(GetMouseLocation());
        }

        private void Update()
        {
            // Show or hide window based on mouse position.

            if (_settings.Disabled || _view.AnimationRunning)
                return;

            if (_view.Opacity < 1)
            {
                Hide(false);
                _view.UpdateUi();
                _view.Opacity = 1;
                return;
            }

            if (_settings.Ignore00XY && (Control.MousePosition.X == 0 && Control.MousePosition.Y == 0))
                return;

            if (FullScreenHelper.DoesMonitorHaveFullscreenApp(_settings.Monitor))
            {
                _showTimer.Stop();
                _hideTimer.Stop();
                Hide(false, true);
                return;
            }

            if (_settings.OpenMode == OpenMode.AlwaysOpen || KeepOpenForIntro)
            {
                Show();
                return;
            }

            if (_view.QueueIntro)
            {
                _view.ShowIntro();
                return;
            }

            if (IsShowable())
            {
                if (_showTimer.IsEnabled == false)
                    _showTimer.Start();
            }
            else
            {
                _showTimer.Stop();
                if (IsHideable())
                {
                    if (_hideTimer.IsEnabled == false)
                        _hideTimer.Start();
                }
                else
                {
                    _hideTimer.Stop();
                }
            }
        }

        private bool IsHideable()
        {
            return !_view.IsMouseOver && _view.IsVisible && !IsMouseInWindowBounds() && !_view.IsContextMenuOpen;
        }

        private bool IsShowable()
        {
            return (_settings.OpenMode == OpenMode.Mouse || _settings.OpenMode == OpenMode.MouseAndKeyboard) &&
                   IsMouseInMouseBounds();
        }

        public void Show(bool animate = true)
        {
            if (_view.AnimationRunning || _view.IsVisible)
                return;
            if (animate && _settings.AnimationTime != 0)
                _view.AnimateSize(AnimationMode.Show);
            else
                _view.Show();
        }

        public void Hide(bool animate = true, bool checkHideStatus = false)
        {
            if (_view.AnimationRunning || !_view.IsVisible)
                return;
            if (checkHideStatus && !IsHideable())
                return;
            KeepOpenForIntro = false;
            if (animate && _settings.AnimationTime != 0)
                _view.AnimateSize(AnimationMode.Hide);
            else
                _view.Hide();
        }
    }
}