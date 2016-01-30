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
using DesktopWidgets.WidgetBase.Settings;
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
            _mouseCheckTimer = new DispatcherTimer();
            _hideTimer = new DispatcherTimer();
            _showTimer = new DispatcherTimer();

            _mouseCheckTimer.Tick += (sender, args) => Update();
            _hideTimer.Tick += delegate
            {
                _hideTimer.Stop();
                Hide();
            };
            _showTimer.Tick += delegate
            {
                _showTimer.Stop();
                Show(activate: true);
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

        public void Start() => _mouseCheckTimer.Start();

        public void Stop() => _mouseCheckTimer.Stop();

        private Rect GetCenterBounds()
        {
            var viewBounds = _view.GetBounds();
            return new Rect(viewBounds.Left - _settings.MouseBounds, viewBounds.Top - _settings.MouseBounds,
                viewBounds.Width + _settings.MouseBounds*2, viewBounds.Height + _settings.MouseBounds*2);
        }

        private bool IsMouseInMouseBounds() => GetValidBounds().Any(checkRect => checkRect.Contains(GetMouseLocation()));

        private bool IsMouseInWindowBounds() => _view.GetBounds().Contains(GetMouseLocation());

        private bool IsMouseInCorners() => _settings.ScreenBounds.GetCorners(_settings.IgnoreScreenCornerSize)
            .Any(x => x.Contains(GetMouseLocation()));

        private IEnumerable<Rect> GetValidBounds()
        {
            if (_settings.CustomMouseDetectionBounds.Width > 0 && _settings.CustomMouseDetectionBounds.Height > 0)
            {
                yield return _settings.CustomMouseDetectionBounds;
            }
            else if (!_settings.IsDocked)
            {
                yield return GetCenterBounds();
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
                            yield return new Rect(_settings.ScreenBounds.Left, _settings.ScreenBounds.Top,
                                _settings.MouseBounds,
                                _settings.ScreenBounds.Height);
                        }
                        if (_settings.HorizontalAlignment == HorizontalAlignment.Right ||
                            _settings.HorizontalAlignment == HorizontalAlignment.Stretch)
                        {
                            yield return new Rect(_settings.ScreenBounds.Right - _settings.MouseBounds,
                                _settings.ScreenBounds.Top, _settings.MouseBounds, _settings.ScreenBounds.Height);
                        }
                        if (_settings.HorizontalAlignment == HorizontalAlignment.Center)
                        {
                            if (_settings.CenterBoundsOnNonSidedDock)
                                yield return GetCenterBounds();
                        }
                    }
                    if (_settings.MouseBoundsDetectionAxis == MouseBoundsDetectionAxis.Both ||
                        _settings.MouseBoundsDetectionAxis == MouseBoundsDetectionAxis.Vertical)
                    {
                        if (_settings.VerticalAlignment == VerticalAlignment.Top ||
                            _settings.VerticalAlignment == VerticalAlignment.Stretch)
                        {
                            yield return new Rect(_settings.ScreenBounds.Left, _settings.ScreenBounds.Top,
                                _settings.ScreenBounds.Width,
                                _settings.MouseBounds);
                        }
                        if (_settings.VerticalAlignment == VerticalAlignment.Bottom ||
                            _settings.VerticalAlignment == VerticalAlignment.Stretch)
                        {
                            yield return new Rect(_settings.ScreenBounds.Left,
                                _settings.ScreenBounds.Bottom - _settings.MouseBounds, _settings.ScreenBounds.Width,
                                _settings.MouseBounds);
                        }
                        if (_settings.VerticalAlignment == VerticalAlignment.Center)
                        {
                            if (_settings.CenterBoundsOnNonSidedDock)
                                yield return GetCenterBounds();
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
                            yield return new Rect(viewBounds.Left, viewBounds.Top, _settings.MouseBounds,
                                viewBounds.Height);
                        }
                        if (_settings.HorizontalAlignment == HorizontalAlignment.Right ||
                            _settings.HorizontalAlignment == HorizontalAlignment.Stretch)
                        {
                            yield return new Rect(viewBounds.Right - _settings.MouseBounds,
                                viewBounds.Top, _settings.MouseBounds, viewBounds.Height);
                        }
                        if (_settings.HorizontalAlignment == HorizontalAlignment.Center)
                        {
                            if (_settings.CenterBoundsOnNonSidedDock)
                                yield return GetCenterBounds();
                        }
                    }
                    if (_settings.MouseBoundsDetectionAxis == MouseBoundsDetectionAxis.Both ||
                        _settings.MouseBoundsDetectionAxis == MouseBoundsDetectionAxis.Vertical)
                    {
                        if (_settings.VerticalAlignment == VerticalAlignment.Top ||
                            _settings.VerticalAlignment == VerticalAlignment.Stretch)
                        {
                            yield return new Rect(viewBounds.Left, viewBounds.Top, viewBounds.Width,
                                _settings.MouseBounds);
                        }
                        if (_settings.VerticalAlignment == VerticalAlignment.Bottom ||
                            _settings.VerticalAlignment == VerticalAlignment.Stretch)
                        {
                            yield return new Rect(viewBounds.Left,
                                viewBounds.Bottom - _settings.MouseBounds, viewBounds.Width,
                                _settings.MouseBounds);
                        }
                        if (_settings.VerticalAlignment == VerticalAlignment.Center)
                        {
                            if (_settings.CenterBoundsOnNonSidedDock)
                                yield return GetCenterBounds();
                        }
                    }
                }
            }
        }

        private void Update()
        {
            if (_settings.Disabled || _view.AnimationRunning)
                return;

            var showIntro = false;
            if (QueueIntro)
            {
                showIntro = true;
                QueueIntro = false;
            }

            if (App.IsMuted)
            {
                Hide(checkHideStatus: false);
                return;
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
                Hide();
                return;
            }

            if (showIntro)
            {
                _view.ShowIntro();
                return;
            }

            UpdateVisibilityStatus();
        }

        private void UpdateVisibilityStatus()
        {
            if (_settings.OpenMode == OpenMode.Mouse || _settings.OpenMode == OpenMode.MouseAndKeyboard)
            {
                if (IsShowable())
                {
                    if (!_view.IsVisible && !(_settings.IgnoreScreenCornerSize != 0 && IsMouseInCorners()))
                    {
                        if (_settings.ShowDelay == 0)
                        {
                            Show(activate: true);
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
            => _view.IsVisible && !_view.IsContextMenuOpen && !(_settings.StayOpenIfMouseFocus && IsShowable());

        private bool IsShowable() => _view.IsMouseOver || IsMouseInMouseBounds();

        public void Show(bool animate = true, bool activate = false)
        {
            if (_view.AnimationRunning || _view.IsVisible || App.IsMuted || _settings.ForceHide)
                return;
            if (animate && _settings.AnimationTime != 0)
                _view.Animate(AnimationMode.Show);
            else
                _view.Show();
            if (activate && _settings.ActivateOnShow)
                _view.Activate();
        }

        public void Hide(bool animate = true, bool checkHideStatus = true)
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
            _mouseCheckTimer = null;
            _hideTimer?.Stop();
            _hideTimer = null;
            _showTimer?.Stop();
            _showTimer = null;
        }
    }
}