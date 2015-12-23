using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using DesktopWidgets.Helpers;
using DesktopWidgets.Widgets.Sidebar;
using Settings = DesktopWidgets.Widgets.CountdownClock.Settings;

namespace DesktopWidgets.Classes
{
    public class BoolConverter<T> : IValueConverter
    {
        protected BoolConverter(T trueValue, T falseValue)
        {
            True = trueValue;
            False = falseValue;
        }

        private T True { get; }
        private T False { get; }

        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterHelper.IsValueValid(value))
                return DependencyProperty.UnsetValue;
            return value is bool && ((bool) value) ? True : False;
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterHelper.IsValueValid(value))
                return DependencyProperty.UnsetValue;
            return value is T && EqualityComparer<T>.Default.Equals((T) value, True);
        }
    }

    public class BoolToThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterHelper.IsValueValid(value))
                return DependencyProperty.UnsetValue;
            var newValue = (bool) value;
            return newValue ? new Thickness(1) : new Thickness(0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SelectedWidgetToEnableDisableNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterHelper.IsValueValid(value))
                return DependencyProperty.UnsetValue;
            var settings = value as WidgetSettingsBase;
            if (settings == null)
                return DependencyProperty.UnsetValue;
            return settings.Disabled ? "Enable" : "Disable";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class NotNullToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DateTimeToCountdownText : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterHelper.IsValueValid(value))
                return DependencyProperty.UnsetValue;
            try
            {
                var val = System.Convert.ToDateTime(value[0]);
                var settings = value[1] as Settings;
                if (settings == null)
                    return DependencyProperty.UnsetValue;

                var format = settings.TimeFormat.Replace(":", "\\:").Replace(".", "\\.");
                var ts = settings.EndDateTime - val;
                return ts.TotalSeconds > 0 || settings.EndContinueCounting
                    ? ts.ToString(format)
                    : TimeSpan.FromSeconds(0).ToString(format);
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DateTimeToFormattedText : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterHelper.IsValueValid(value))
                return DependencyProperty.UnsetValue;
            try
            {
                var val = System.Convert.ToDateTime(value[0]);
                var settings = value[1] as Widgets.TimeClock.Settings;
                if (settings == null)
                    return DependencyProperty.UnsetValue;

                var format = settings.TimeFormat.Replace(":", "\\:").Replace(".", "\\.");
                return val.ToString(format);
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MultiBoolToVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterHelper.IsValueValid(value))
                return DependencyProperty.UnsetValue;
            try
            {
                return value.Cast<bool>().All(x => x) ? Visibility.Visible : Visibility.Collapsed;
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StringToIsNotNullOrEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !string.IsNullOrEmpty((string) value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StringToIsNotNullOrWhiteSpaceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !string.IsNullOrWhiteSpace((string) value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolToStartStopTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterHelper.IsValueValid(value))
                return DependencyProperty.UnsetValue;
            return ((bool) value) ? "Stop" : "Start";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DateTimeToElapsedTimeTextConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterHelper.IsValueValid(value))
                return DependencyProperty.UnsetValue;
            try
            {
                var currentTime = System.Convert.ToDateTime(value[0]);
                var startTime = System.Convert.ToDateTime(value[1]);
                var settings = value[2] as Widgets.StopwatchClock.Settings;
                if (settings == null)
                    return DependencyProperty.UnsetValue;

                var format = settings.TimeFormat.Replace(":", "\\:").Replace(".", "\\.");
                var ts = startTime - currentTime;
                return ts.ToString(format);
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ShowIconContentToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterHelper.IsValueValid(value))
                return DependencyProperty.UnsetValue;
            var newValue = (ShortcutContentMode) value;
            return (newValue == ShortcutContentMode.Icon || newValue == ShortcutContentMode.Both)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ShowTextContentToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterHelper.IsValueValid(value))
                return DependencyProperty.UnsetValue;
            var newValue = (ShortcutContentMode) value;
            return (newValue == ShortcutContentMode.Text || newValue == ShortcutContentMode.Both)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ScrollBarVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterHelper.IsValueValid(value))
                return DependencyProperty.UnsetValue;
            return ((ScrollBarVisibility) value).ToWindowsScrollBarVisibility();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ShortcutToImageConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterHelper.IsValueValid(value))
                return DependencyProperty.UnsetValue;
            var shortcut = value[0] as Shortcut;
            var viewModel = value[1] as Widgets.Sidebar.ViewModel;
            if (viewModel == null || shortcut == null)
                return DependencyProperty.UnsetValue;

            return shortcut.GetShortcutIcon(viewModel);
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IconScalingModeToBitmapScalingModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterHelper.IsValueValid(value))
                return DependencyProperty.UnsetValue;
            return ((ImageScalingMode) value).ToBitmapScalingMode();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IconPositionTextToDockConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterHelper.IsValueValid(value))
                return DependencyProperty.UnsetValue;
            switch ((IconPosition) value)
            {
                default:
                case IconPosition.Right:
                    return Dock.Left;
                case IconPosition.Left:
                    return Dock.Right;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IconPositionIconToDockConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterHelper.IsValueValid(value))
                return DependencyProperty.UnsetValue;
            switch ((IconPosition) value)
            {
                default:
                case IconPosition.Left:
                    return Dock.Left;
                case IconPosition.Right:
                    return Dock.Right;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ShortcutToToolTipConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterHelper.IsValueValid(value))
                return DependencyProperty.UnsetValue;
            var shortcut = value[0] as Shortcut;
            var val = value[1] as Widgets.Sidebar.Settings;
            if (val == null || shortcut == null)
                return DependencyProperty.UnsetValue;

            switch (val.ToolTipType)
            {
                case ToolTipType.Name:
                    return shortcut.Name;
                case ToolTipType.Path:
                    return shortcut.Path;
                case ToolTipType.Both:
                    return $"{shortcut.Name}\n{shortcut.Path}";
                default:
                    return null;
            }
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TextToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterHelper.IsValueValid(value))
                return DependencyProperty.UnsetValue;
            var val = value as string;
            if (val == null)
                return DependencyProperty.UnsetValue;
            return string.IsNullOrWhiteSpace(val) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PositionToOrientationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterHelper.IsValueValid(value))
                return DependencyProperty.UnsetValue;
            var val = value as Widgets.Sidebar.Settings;
            if (val == null)
                return DependencyProperty.UnsetValue;
            switch (val.ShortcutOrientation)
            {
                default:
                case ShortcutOrientation.Auto:
                    return val.DockPosition.IsVertical()
                        ? Orientation.Horizontal
                        : Orientation.Vertical;
                case ShortcutOrientation.Horizontal:
                    return Orientation.Horizontal;
                case ShortcutOrientation.Vertical:
                    return Orientation.Vertical;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SettingsToHorizontalAlignmentConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterHelper.IsValueValid(value))
                return DependencyProperty.UnsetValue;
            try
            {
                var dockPosition = (ScreenDockPosition) value[0];
                var shortcutAlignment = (ShortcutAlignment) value[1];
                return dockPosition.IsVertical()
                    ? shortcutAlignment.ToHorizontalAlignment()
                    : HorizontalAlignment.Stretch;
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SettingsToVerticalAlignmentConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterHelper.IsValueValid(value))
                return DependencyProperty.UnsetValue;
            try
            {
                var dockPosition = (ScreenDockPosition) value[0];
                var shortcutAlignment = (ShortcutAlignment) value[1];
                return dockPosition.IsHorizontal() ? shortcutAlignment.ToVerticalAlignment() : VerticalAlignment.Top;
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ShortcutToManageNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterHelper.IsValueValid(value))
                return DependencyProperty.UnsetValue;
            var shortcut = value as Shortcut;
            if (shortcut == null)
                return DependencyProperty.UnsetValue;
            return
                $"{shortcut.Name}{((string.IsNullOrWhiteSpace(shortcut.Path) && string.IsNullOrWhiteSpace(shortcut.Args)) ? "" : (($"{shortcut.Path}{(shortcut.Args == "" ? "" : $", {shortcut.Args}")}")))}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolToEnableDisableNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterHelper.IsValueValid(value))
                return DependencyProperty.UnsetValue;
            return (bool) value ? "Enable" : "Disable";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolToCollapsedVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterHelper.IsValueValid(value))
                return DependencyProperty.UnsetValue;
            return (bool) value ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class WidgetToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterHelper.IsValueValid(value))
                return DependencyProperty.UnsetValue;
            return (value as WidgetSettingsBase)?.Identifier?.GetName();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MaxSizeConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterHelper.IsValueValid(value))
                return DependencyProperty.UnsetValue;
            try
            {
                var baseAmount = (double) value[0];
                var total = value.ToList().GetRange(1, value.Length - 3).Select(x => (double) x).Sum();
                if ((bool) value[value.Length - 1])
                    total += (double) value[value.Length - 2];
                return baseAmount - total;
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IsExpandedToMoreOptionsHeaderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterHelper.IsValueValid(value))
                return DependencyProperty.UnsetValue;
            return (bool) value ? "Less Options" : "More Options";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}