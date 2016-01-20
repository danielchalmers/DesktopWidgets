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
        private readonly WidgetSettingsBase _settings;
        private readonly WidgetView _view;
        private DispatcherTimer _hideTimer;
        private DispatcherTimer _mouseCheckTimer;
        private DispatcherTimer _showTimer;
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
                    if (_settings.MouseBoundsDetectionAxis == MouseBoundsDetectionAxis.Both ||
                        _settings.MouseBoundsDetectionAxis == MouseBoundsDetectionAxis.Horizontal)
                    {
                        if (_settings.HorizontalAlignment == HorizontalAlignment.Left ||
                            _settings.HorizontalAlignment == HorizontalAlignment.Stretch)
                        {
                            checkBounds.Add(new Rect(_settings.ScreenBounds.Left, _settings.ScreenBounds.Top,
                                _settings.MouseBounds,
                                _settings.ScreenBounds.Height));
                        }
                        if (_settings.HorizontalAlignment == HorizontalAlignment.Right ||
                            _settings.HorizontalAlignment == HorizontalAlignment.Stretch)
                        {
                            checkBounds.Add(new Rect(_settings.ScreenBounds.Right - _settings.MouseBounds,
                                _settings.ScreenBounds.Top, _settings.MouseBounds, _settings.ScreenBounds.Height));
                        }
                        if (_settings.HorizontalAlignment == HorizontalAlignment.Center)
                        {
                            if (_settings.CenterBoundsOnNonSidedDock)
                                checkBounds.Add(GetCenterBounds());
                        }
                    }
                    if (_settings.MouseBoundsDetectionAxis == MouseBoundsDetectionAxis.Both ||
                        _settings.MouseBoundsDetectionAxis == MouseBoundsDetectionAxis.Vertical)
                    {
                        if (_settings.VerticalAlignment == VerticalAlignment.Top ||
                            _settings.VerticalAlignment == VerticalAlignment.Stretch)
                        {
                            checkBounds.Add(new Rect(_settings.ScreenBounds.Left, _settings.ScreenBounds.Top,
                                _settings.ScreenBounds.Width,
                                _settings.MouseBounds));
                        }
                        if (_settings.VerticalAlignment == VerticalAlignment.Bottom ||
                            _settings.VerticalAlignment == VerticalAlignment.Stretch)
                        {
                            checkBounds.Add(new Rect(_settings.ScreenBounds.Left,
                                _settings.ScreenBounds.Bottom - _settings.MouseBounds, _settings.ScreenBounds.Width,
                                _settings.MouseBounds));
                        }
                        if (_settings.VerticalAlignment == VerticalAlignment.Center)
                        {
                            if (_settings.CenterBoundsOnNonSidedDock)
                                checkBounds.Add(GetCenterBounds());
                        }
                    }
                }
                else
                {
                    var viewBounds = _view.GetBounds();
                    if (_settings.MouseBoundsDetectionAxis == MouseBoundsDetectionAxis.Both ||
                        _settings.MouseBoundsDetectionAxis == MouseBoundsDetectionAxis.Horizontal)
                    {
                        if (_settings.HorizontalAlignment == HorizontalAlignment.Left ||
                            _settings.HorizontalAlignment == HorizontalAlignment.Stretch)
                        {
                            checkBounds.Add(new Rect(viewBounds.Left, viewBounds.Top, _settings.MouseBounds,
                                viewBounds.Height));
                        }
                        if (_settings.HorizontalAlignment == HorizontalAlignment.Right ||
                            _settings.HorizontalAlignment == HorizontalAlignment.Stretch)
                        {
                            checkBounds.Add(new Rect(viewBounds.Right - _settings.MouseBounds,
                                viewBounds.Top, _settings.MouseBounds, viewBounds.Height));
                        }
                        if (_settings.HorizontalAlignment == HorizontalAlignment.Center)
                        {
                            if (_settings.CenterBoundsOnNonSidedDock)
                                checkBounds.Add(GetCenterBounds());
                        }
                    }
                    if (_settings.MouseBoundsDetectionAxis == MouseBoundsDetectionAxis.Both ||
                        _settings.MouseBoundsDetectionAxis == MouseBoundsDetectionAxis.Vertical)
                    {
                        if (_settings.VerticalAlignment == VerticalAlignment.Top ||
                            _settings.VerticalAlignment == VerticalAlignment.Stretch)
                        {
                            checkBounds.Add(new Rect(viewBounds.Left, viewBounds.Top, viewBounds.Width,
                                _settings.MouseBounds));
                        }
                        if (_settings.VerticalAlignment == VerticalAlignment.Bottom ||
                            _settings.VerticalAlignment == VerticalAlignment.Stretch)
                        {
                            checkBounds.Add(new Rect(viewBounds.Left,
                                viewBounds.Bottom - _settings.MouseBounds, viewBounds.Width,
                                _settings.MouseBounds));
                        }
                        if (_settings.VerticalAlignment == VerticalAlignment.Center)
                        {
                            if (_settings.CenterBoundsOnNonSidedDock)
                                checkBounds.Add(GetCenterBounds());
                        }
                    }
                }
            }

            return checkBounds.Any(checkRect => checkRect.Contains(GetMouseLocation()));
        }

        private bool IsMouseInWindowBounds()
        {
            return _view.GetBounds().Contains(GetMouseLocation());
        }

        private bool IsMouseInCorners()
        {
            return
                _settings.ScreenBounds.GetCorners(_settings.IgnoreScreenCornerSize)
                    .Any(x => x.Contains(GetMouseLocation()));
        }

        private void Update()
        {
            // Show or hide window based on mouse position.

            if (_settings.Disabled || _view.AnimationRunning)
                return;

            if (App.IsMuted)
            {
                Hide();
                return;
            }

            var showIntro = false;
            if (QueueIntro)
            {
                showIntro = true;
                QueueIntro = false;
            }

            if (!_settings.FullscreenActivation && FullScreenHelper.DoesMonitorHaveFullscreenApp(_settings.ScreenBounds))
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

            if (_settings.OpenMode == OpenMode.Mouse || _settings.OpenMode == OpenMode.MouseAndKeyboard)
            {
                if (IsShowable())
                {
                    if (!_view.IsVisible &&
                        !(_settings.Ignore00XY && Control.MousePosition.X == 0 && Control.MousePosition.Y == 0) &&
                        !(_settings.IgnoreScreenCornerSize != 0 && IsMouseInCorners()))
                    {
                        if (_settings.ShowDelay == 0)
                        {
                            Show();
                        }
                        else
                        {
                            if (_showTimer.IsEnabled == false)
                                _showTimer.Start();
                        }
                    }
                }
                else
                {
                    if (_showTimer.IsEnabled)
                        _showTimer.Stop();
                    if (IsHideable())
                    {
                        if (_settings.HideDelay == 0)
                        {
                            Hide();
                        }
                        else
                        {
                            if (_hideTimer.IsEnabled == false)
                                _hideTimer.Start();
                        }
                    }
                    else
                    {
                        if (_hideTimer.IsEnabled)
                            _hideTimer.Stop();
                    }
                }
            }
        }

        private bool IsHideable()
        {
            return _view.IsVisible && !_view.IsContextMenuOpen && !(_settings.StayOpenIfMouseFocus && IsShowable());
        }

        private bool IsShowable()
        {
            return _view.IsMouseOver || IsMouseInMouseBounds();
        }

        public void Show(bool animate = true)
        {
            if (_view.AnimationRunning || _view.IsVisible)
                return;
            if (animate && _settings.AnimationTime != 0)
                _view.Animate(AnimationMode.Show);
            else
                _view.Show();
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
                _view.Hide();
        }

        public void Dispose()
        {
            _mouseCheckTimer?.Stop();
            _hideTimer?.Stop();
            _showTimer?.Stop();
            _mouseCheckTimer = null;
            _hideTimer = null;
            _showTimer = null;
        }
    }
}