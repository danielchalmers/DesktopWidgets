#region

using System;
using System.Collections.Generic;
using System.Linq;
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

        public bool IsRunning => _mouseCheckTimer.IsEnabled;

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
            var checkBounds = new List<Rect>();
            if (_settings.CustomMouseDetectionBounds.Width > 0 && _settings.CustomMouseDetectionBounds.Height > 0)
            {
                checkBounds.Add(_settings.CustomMouseDetectionBounds);
            }
            else if (_settings.DockPosition == ScreenDockPosition.None)
            {
                var viewBounds = _view.GetBounds();
                checkBounds.Add(new Rect(viewBounds.Left - _settings.MouseBounds, viewBounds.Top - _settings.MouseBounds,
                    viewBounds.Width + (_settings.MouseBounds*2), viewBounds.Height + (_settings.MouseBounds*2)));
            }
            else
            {
                if (_settings.StretchBounds)
                {
                    var monitorBounds = MonitorHelper.GetMonitorBounds(_settings.Monitor);
                    if (_settings.DockPosition == ScreenDockPosition.Left ||
                        _settings.DockAlignment == ScreenDockAlignment.Top)
                    {
                        checkBounds.Add(new Rect(monitorBounds.Left, monitorBounds.Top, _settings.MouseBounds,
                            monitorBounds.Height));
                    }
                    if (_settings.DockPosition == ScreenDockPosition.Right ||
                        _settings.DockAlignment == ScreenDockAlignment.Bottom)
                    {
                        checkBounds.Add(new Rect((monitorBounds.Left + _view.ActualWidth) - _settings.MouseBounds,
                            monitorBounds.Top, _settings.MouseBounds, monitorBounds.Height));
                    }
                    if (_settings.DockPosition == ScreenDockPosition.Top ||
                        _settings.DockAlignment == ScreenDockAlignment.Top)
                    {
                        checkBounds.Add(new Rect(monitorBounds.Left, monitorBounds.Top, monitorBounds.Width,
                            _settings.MouseBounds));
                    }
                    if (_settings.DockPosition == ScreenDockPosition.Bottom ||
                        _settings.DockAlignment == ScreenDockAlignment.Bottom)
                    {
                        checkBounds.Add(new Rect(monitorBounds.Left,
                            (monitorBounds.Top + _view.ActualHeight) - _settings.MouseBounds, monitorBounds.Width,
                            _settings.MouseBounds));
                    }
                }
                else
                {
                    var viewBounds = _view.GetBounds();
                    if (_settings.DockPosition == ScreenDockPosition.Left ||
                        _settings.DockAlignment == ScreenDockAlignment.Top)
                    {
                        checkBounds.Add(new Rect(viewBounds.Left, viewBounds.Top, _settings.MouseBounds,
                            viewBounds.Height));
                    }
                    if (_settings.DockPosition == ScreenDockPosition.Right ||
                        _settings.DockAlignment == ScreenDockAlignment.Bottom)
                    {
                        checkBounds.Add(new Rect((viewBounds.Left + _view.ActualWidth) - _settings.MouseBounds,
                            viewBounds.Top, _settings.MouseBounds, viewBounds.Height));
                    }
                    if (_settings.DockPosition == ScreenDockPosition.Top ||
                        _settings.DockAlignment == ScreenDockAlignment.Top)
                    {
                        checkBounds.Add(new Rect(viewBounds.Left, viewBounds.Top, viewBounds.Width,
                            _settings.MouseBounds));
                    }
                    if (_settings.DockPosition == ScreenDockPosition.Bottom ||
                        _settings.DockAlignment == ScreenDockAlignment.Bottom)
                    {
                        checkBounds.Add(new Rect(viewBounds.Left,
                            (viewBounds.Top + _view.ActualHeight) - _settings.MouseBounds, viewBounds.Width,
                            _settings.MouseBounds));
                    }
                }
            }

            return checkBounds.Any(x => x.Contains(GetMouseLocation()));
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

            if (_view.IsRefreshRequired)
            {
                _view.UpdateUi(false, false);
                return;
            }

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

            if (_settings.OpenMode == OpenMode.Hidden)
            {
                Hide();
                return;
            }

            if (_view.QueueIntro)
            {
                _view.ShowIntro();
                return;
            }

            if (!(_settings.Ignore00XY && (Control.MousePosition.X == 0 && Control.MousePosition.Y == 0)))
            {
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
                _view.Animate(AnimationMode.Show);
            else
                _view.ShowOpacity();
        }

        public void Hide(bool animate = true, bool checkHideStatus = false)
        {
            if (_view.AnimationRunning || !_view.IsVisible)
                return;
            if (checkHideStatus && !IsHideable())
                return;
            KeepOpenForIntro = false;
            if (animate && _settings.AnimationTime != 0)
                _view.Animate(AnimationMode.Hide);
            else
                _view.HideOpacity();
        }
    }
}