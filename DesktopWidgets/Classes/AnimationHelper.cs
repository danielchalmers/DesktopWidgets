#region

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using DesktopWidgets.Helpers;
using DesktopWidgets.View;

#endregion

namespace DesktopWidgets.Classes
{
    public static class AnimationHelper
    {
        public static void AnimateSize(this WidgetView view, AnimationMode animationMode, Action astartAction = null,
            Action aendAction = null)
        {
            var settings = view.Id.GetSettings();

            Action startAction = delegate
            {
                if (animationMode == AnimationMode.Show)
                    view.Show();
                astartAction?.Invoke();
                view.AnimationRunning = true;
            };
            Action finishAction = delegate
            {
                if (animationMode == AnimationMode.Hide)
                    view.Hide();
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
            doubleAnimation.Completed += (sender, args) => finishAction();

            switch (settings.DockPosition)
            {
                default:
                    view.RenderTransformOrigin = new Point(0.5, 0.5);
                    break;
                case ScreenDockPosition.Left:
                    view.RenderTransformOrigin = new Point(0, 1);
                    break;
                case ScreenDockPosition.Right:
                    view.RenderTransformOrigin = new Point(1, 0);
                    break;
                case ScreenDockPosition.Top:
                    view.RenderTransformOrigin = new Point(1, 0);
                    break;
                case ScreenDockPosition.Bottom:
                    view.RenderTransformOrigin = new Point(0, 1);
                    break;
            }

            // Start animation.
            startAction();
            switch (settings.AnimationType)
            {
                case AnimationType.Fade:
                    var storyBoard = new Storyboard();
                    storyBoard.Children.Add(doubleAnimation);
                    Storyboard.SetTarget(doubleAnimation, view);
                    Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(UIElement.OpacityProperty));
                    storyBoard.Begin();
                    break;
                case AnimationType.Slide:
                    var trans = new ScaleTransform();
                    view.RenderTransform = trans;
                    trans.BeginAnimation(
                        settings.DockPosition.IsVertical()
                            ? ScaleTransform.ScaleYProperty
                            : ScaleTransform.ScaleXProperty, doubleAnimation);
                    break;
                default:
                    finishAction();
                    break;
            }
        }
    }
}