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
using HorizontalAlignment = System.Windows.HorizontalAlignment;

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
        public bool QueueIntro;

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

        private Rect GetCenterBounds()
        {
            var viewBounds = _view.GetBounds();
            return new Rect(viewBounds.Left - _settings.MouseBounds, viewBounds.Top - _settings.MouseBounds,
                viewBounds.Width + _settings.MouseBounds*2, viewBounds.Height + _settings.MouseBounds*2);
        }

        private bool IsMouseInMouseBounds()
        {
            // Return is mouse in correct bounds to show sidebar.
            var checkBounds = new List<Rect>();
            if (_settings.CustomMouseDetectionBounds.Width > 0 && _settings.CustomMouseDetectionBounds.Height > 0)
            {
                checkBounds.Add(_settings.CustomMouseDetectionBounds);
            }
            else if (!_settings.IsDocked)
            {
                checkBounds.Add(GetCenterBounds());
            }
            else
            {
                if (_settings.StretchBounds)
                {
                    var monitorBounds = MonitorHelper.GetMonitorBounds(_settings.Monitor);
                    switch (_settings.HorizontalAlignment)
                    {
                        case HorizontalAlignment.Left:
                            checkBounds.Add(new Rect(monitorBounds.Left, monitorBounds.Top, _settings.MouseBounds,
                                monitorBounds.Height));
                            break;
                        case HorizontalAlignment.Right:
                            checkBounds.Add(new Rect(monitorBounds.Left + _view.ActualWidth - _settings.MouseBounds,
                                monitorBounds.Top, _settings.MouseBounds, monitorBounds.Height));
                            break;
                        default:
                            if (_settings.CenterBoundsOnNonSidedDock)
                                checkBounds.Add(GetCenterBounds());
                            break;
                    }
                    switch (_settings.VerticalAlignment)
                    {
                        case VerticalAlignment.Top:
                            checkBounds.Add(new Rect(monitorBounds.Left, monitorBounds.Top, monitorBounds.Width,
                                _settings.MouseBounds));
                            break;
                        case VerticalAlignment.Bottom:
                            checkBounds.Add(new Rect(monitorBounds.Left,
                                monitorBounds.Top + _view.ActualHeight - _settings.MouseBounds, monitorBounds.Width,
                                _settings.MouseBounds));
                            break;
                        default:
                            if (_settings.CenterBoundsOnNonSidedDock)
                                checkBounds.Add(GetCenterBounds());
                            break;
                    }
                }
                else
                {
                    var viewBounds = _view.GetBounds();
                    switch (_settings.HorizontalAlignment)
                    {
                        case HorizontalAlignment.Left:
                            checkBounds.Add(new Rect(viewBounds.Left, viewBounds.Top, _settings.MouseBounds,
                                viewBounds.Height));
                            break;
                        case HorizontalAlignment.Right:
                            checkBounds.Add(new Rect(viewBounds.Left + _view.ActualWidth - _settings.MouseBounds,
                                viewBounds.Top, _settings.MouseBounds, viewBounds.Height));
                            break;
                        default:
                            if (_settings.CenterBoundsOnNonSidedDock)
                                checkBounds.Add(GetCenterBounds());
                            break;
                    }
                    switch (_settings.VerticalAlignment)
                    {
                        case VerticalAlignment.Top:
                            checkBounds.Add(new Rect(viewBounds.Left, viewBounds.Top, viewBounds.Width,
                                _settings.MouseBounds));
                            break;
                        case VerticalAlignment.Bottom:
                            checkBounds.Add(new Rect(viewBounds.Left,
                                viewBounds.Top + _view.ActualHeight - _settings.MouseBounds, viewBounds.Width,
                                _settings.MouseBounds));
                            break;
                        default:
                            if (_settings.CenterBoundsOnNonSidedDock)
                                checkBounds.Add(GetCenterBounds());
                            break;
                    }
                }
            }

            return checkBounds.Any(checkRect => checkRect.Contains(GetMouseLocation()));
        }

        private bool IsMouseInWindowBounds()
        {
            return _view.GetBounds().Contains(GetMouseLocation());
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

            var showIntro = false;
            if (QueueIntro)
            {
                showIntro = true;
                QueueIntro = false;
            }

            if (!_settings.FullscreenActivation && FullScreenHelper.DoesMonitorHaveFullscreenApp(_settings.Monitor))
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
                if (_settings.StayOpenIfMouseFocus)
                {
                    if (!IsMouseInWindowBounds())
                        Hide();
                }
                else
                {
                    Hide();
                }
                return;
            }

            if (showIntro)
            {
                _view.ShowIntro();
                return;
            }

            if (!(_settings.Ignore00XY && Control.MousePosition.X == 0 && Control.MousePosition.Y == 0))
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
            var hideable = !_view.IsMouseOver && _view.IsVisible && !_view.IsContextMenuOpen;
            if (hideable && _settings.StayOpenIfMouseFocus && IsMouseInWindowBounds()) hideable = false;
            return hideable;
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
            KeepOpenForIntro = false;
            if (_view.AnimationRunning || !_view.IsVisible)
                return;
            if (checkHideStatus && !IsHideable())
                return;
            if (animate && _settings.AnimationTime != 0)
                _view.Animate(AnimationMode.Hide);
            else
                _view.HideOpacity();
        }
    }
}