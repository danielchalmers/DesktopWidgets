#region

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using DesktopWidgets.Classes;
using DesktopWidgets.View;

#endregion

namespace DesktopWidgets.Helpers
{
    public static class AnimationHelper
    {
        public static void Animate(this WidgetView view, AnimationMode animationMode, Action astartAction = null,
            Action aendAction = null, ScreenDockPosition? dockPosition = null, ScreenDockAlignment? dockAlignment = null)
        {
            var settings = view.Id.GetSettings();

            var twoAnimations = true;

            var position = dockPosition ?? settings.DockPosition;
            var alignment = dockAlignment ?? settings.DockAlignment;

            switch (position)
            {
                default:
                    view.RenderTransformOrigin = new Point(0.5, 0.5);
                    break;
                case ScreenDockPosition.Left:
                    switch (alignment)
                    {
                        default:
                            view.RenderTransformOrigin = new Point(0, 0);
                            twoAnimations = false;
                            break;
                        case ScreenDockAlignment.Top:
                            view.RenderTransformOrigin = new Point(0, 0);
                            break;
                        case ScreenDockAlignment.Bottom:
                            view.RenderTransformOrigin = new Point(0, 1);
                            break;
                    }
                    break;
                case ScreenDockPosition.Right:
                    switch (alignment)
                    {
                        default:
                            view.RenderTransformOrigin = new Point(1, 0);
                            twoAnimations = false;
                            break;
                        case ScreenDockAlignment.Top:
                            view.RenderTransformOrigin = new Point(1, 0);
                            break;
                        case ScreenDockAlignment.Bottom:
                            view.RenderTransformOrigin = new Point(1, 1);
                            break;
                    }
                    break;
                case ScreenDockPosition.Top:
                    switch (alignment)
                    {
                        default:
                            view.RenderTransformOrigin = new Point(0, 0);
                            twoAnimations = false;
                            break;
                        case ScreenDockAlignment.Top:
                            view.RenderTransformOrigin = new Point(0, 0);
                            break;
                        case ScreenDockAlignment.Bottom:
                            view.RenderTransformOrigin = new Point(1, 0);
                            break;
                    }
                    break;
                case ScreenDockPosition.Bottom:
                    switch (alignment)
                    {
                        default:
                            view.RenderTransformOrigin = new Point(0, 1);
                            twoAnimations = false;
                            break;
                        case ScreenDockAlignment.Top:
                            view.RenderTransformOrigin = new Point(0, 1);
                            break;
                        case ScreenDockAlignment.Bottom:
                            view.RenderTransformOrigin = new Point(1, 1);
                            break;
                    }
                    break;
            }

            Action startAction = delegate
            {
                if (animationMode == AnimationMode.Show)
                {
                    view.ShowOpacity();
                    SoundHelper.PlaySoundAsync(settings.ShowSoundPath, settings.ShowSoundVolume);
                }
                astartAction?.Invoke();
                view.AnimationRunning = true;
            };
            Action finishAction = delegate
            {
                if (animationMode == AnimationMode.Hide)
                {
                    view.HideOpacity();
                    SoundHelper.PlaySoundAsync(settings.HideSoundPath, settings.HideSoundVolume);
                }
                aendAction?.Invoke();
                view.AnimationRunning = false;
            };

            if (settings.AnimationType == AnimationType.None)
            {
                startAction();
                finishAction();
                return;
            }

            var doubleAnimation = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(settings.AnimationTime)),
                From = animationMode == AnimationMode.Show ? 0 : 1,
                To = animationMode == AnimationMode.Show ? 1 : 0,
                FillBehavior = FillBehavior.Stop,
                EasingFunction =
                    settings.AnimationEase
                        ? new SineEase
                        {
                            EasingMode = animationMode == AnimationMode.Show ? EasingMode.EaseIn : EasingMode.EaseOut
                        }
                        : null
            };

            // Start animation.
            startAction();
            switch (settings.AnimationType)
            {
                case AnimationType.Fade:
                    var storyBoard = new Storyboard();
                    storyBoard.Completed += (sender, args) => finishAction();
                    storyBoard.Children.Add(doubleAnimation);
                    Storyboard.SetTarget(doubleAnimation, view);
                    Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(UIElement.OpacityProperty));
                    storyBoard.Begin();
                    break;
                case AnimationType.Slide:
                    var counter = 0;
                    doubleAnimation.Completed += delegate
                    {
                        counter++;
                        if (!twoAnimations || counter == 2)
                            finishAction();
                    };
                    var trans = new ScaleTransform();
                    view.RenderTransform = trans;
                    if (twoAnimations)
                    {
                        trans.BeginAnimation(ScaleTransform.ScaleXProperty, doubleAnimation);
                        trans.BeginAnimation(ScaleTransform.ScaleYProperty, doubleAnimation);
                    }
                    else
                    {
                        trans.BeginAnimation(
                            position.IsVertical()
                                ? ScaleTransform.ScaleYProperty
                                : ScaleTransform.ScaleXProperty, doubleAnimation);
                    }
                    break;
                default:
                    finishAction();
                    break;
            }
        }
    }
}