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
        public static void Animate(this WidgetView view, AnimationMode animationMode,
            Action astartAction = null,
            Action aendAction = null,
            bool? isDocked = null,
            HorizontalAlignment? dockHorizontalAlignment = null,
            VerticalAlignment? dockVerticalAlignment = null)
        {
            var settings = view.Id.GetSettings();

            var horizontalAlignment = dockHorizontalAlignment ?? settings.HorizontalAlignment;
            var verticalAlignment = dockVerticalAlignment ?? settings.VerticalAlignment;
            var docked = isDocked ?? settings.IsDocked;

            view.RenderTransformOrigin = new Point(0.5, 0.5);
            if (docked)
            {
                switch (horizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        view.RenderTransformOrigin = new Point(0, view.RenderTransformOrigin.Y);
                        break;
                    case HorizontalAlignment.Right:
                        view.RenderTransformOrigin = new Point(1, view.RenderTransformOrigin.Y);
                        break;
                }
                switch (verticalAlignment)
                {
                    case VerticalAlignment.Top:
                        view.RenderTransformOrigin = new Point(view.RenderTransformOrigin.X, 0);
                        break;
                    case VerticalAlignment.Bottom:
                        view.RenderTransformOrigin = new Point(view.RenderTransformOrigin.X, 1);
                        break;
                }
            }

            Action startAction = delegate
            {
                if (animationMode == AnimationMode.Show)
                {
                    view.Show();
                    MediaPlayerStore.PlaySoundAsync(settings.ShowSoundPath, settings.ShowSoundVolume);
                }
                astartAction?.Invoke();
                view.AnimationRunning = true;
            };
            Action finishAction = delegate
            {
                if (animationMode == AnimationMode.Hide)
                {
                    view.Hide();
                    MediaPlayerStore.PlaySoundAsync(settings.HideSoundPath, settings.HideSoundVolume);
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
                EasingFunction = settings.AnimationEase
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
                        if (counter == 2)
                            finishAction();
                    };
                    var trans = new ScaleTransform();
                    view.RenderTransform = trans;
                    trans.BeginAnimation(ScaleTransform.ScaleXProperty, doubleAnimation);
                    trans.BeginAnimation(ScaleTransform.ScaleYProperty, doubleAnimation);
                    break;
                default:
                    finishAction();
                    break;
            }
        }
    }
}