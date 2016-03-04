using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using DesktopWidgets.Helpers;
using DesktopWidgets.WidgetBase.Settings;
using DesktopWidgets.Widgets.RSSFeed;
using DesktopWidgets.Widgets.Sidebar;
using Settings = DesktopWidgets.Widgets.CountdownClock.Settings;
using ViewModel = DesktopWidgets.Widgets.Sidebar.ViewModel;

namespace DesktopWidgets.Classes
{
    public class SettingsToThicknessConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterHelper.IsValueValid(value))
                return DependencyProperty.UnsetValue;
            if (value.Length != 2)
                return DependencyProperty.UnsetValue;
            var enabled = (bool) value[0];
            return enabled ? value[1] : new Thickness(0);
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
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
            var val = System.Convert.ToDateTime(value[0]);
            var settings = value[1] as Settings;
            if (settings == null)
                return DependencyProperty.UnsetValue;

            var ts = settings.EndDateTime - val;
            return ts.TotalSeconds > 0 || settings.EndContinueCounting
                ? ts.ParseCustomFormat(settings.DateTimeFormat)
                : TimeSpan.FromSeconds(0).ParseCustomFormat(settings.DateTimeFormat);
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
            var dateTime = System.Convert.ToDateTime(value[0]);
            var format = (List<string>) value[1];
            return dateTime.ParseCustomFormat(format);
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
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
            var currentTime = System.Convert.ToDateTime(value[0]);
            var startTime = System.Convert.ToDateTime(value[1]);
            var format = (List<string>) value[2];
            return (startTime - currentTime).ParseCustomFormat(format);
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

    public class BoolToStartStopTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterHelper.IsValueValid(value))
                return DependencyProperty.UnsetValue;
            return (bool) value ? "Stop" : "Start";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
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
            return newValue == ShortcutContentMode.Icon || newValue == ShortcutContentMode.Both
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
            return newValue == ShortcutContentMode.Text || newValue == ShortcutContentMode.Both
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
            var viewModel = value[1] as ViewModel;
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
                    return shortcut.ProcessFile.Path;
                case ToolTipType.Both:
                    return $"{shortcut.Name}\n{shortcut.ProcessFile.Path}";
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
                case ShortcutOrientation.Horizontal:
                    return Orientation.Horizontal;
                case ShortcutOrientation.Vertical:
                    return Orientation.Vertical;
                default:
                    return DependencyProperty.UnsetValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
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
            return shortcut.GetFriendlyNameWithData();
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

    public class WidgetToNameConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterHelper.IsValueValid(value))
                return DependencyProperty.UnsetValue;
            return (value[0] as WidgetSettingsBase)?.Identifier?.GetName();
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
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
                var total = 0.0;
                var ignoreNext = false;
                foreach (var val in value.ToList().GetRange(1, value.Length - 6))
                {
                    if (ignoreNext)
                        continue;
                    if (val is bool)
                    {
                        if (!(bool) val)
                            ignoreNext = true;
                    }
                    else
                    {
                        total += (double) val;
                    }
                }

                var condition1 = (bool) value[value.Length - 5];
                var condition2 = (bool) value[value.Length - 4];
                var actualSize = (double) value[value.Length - 3];
                var size1 = (double) value[value.Length - 2];
                var size2 = (double) value[value.Length - 1];

                var baseReturn = baseAmount - total;

                if (condition1 && condition2)
                    baseReturn -= actualSize + size1 + size2;

                return baseReturn;
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

    public class SizePaddingConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterHelper.IsValueValid(value))
                return DependencyProperty.UnsetValue;
            try
            {
                var baseAmount = (double) value[0];
                var total = 0.0;
                var ignoreNext = false;
                foreach (var val in value.ToList().GetRange(1, value.Length - 1))
                {
                    if (ignoreNext)
                        continue;
                    if (val is bool)
                    {
                        if (!(bool) val)
                            ignoreNext = true;
                    }
                    else
                    {
                        total += (double) val;
                    }
                }
                return baseAmount + total;
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

    public class TimeSpanToMuteTextConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterHelper.IsValueValid(value))
                return DependencyProperty.UnsetValue;
            var timeSpan = (TimeSpan) value[0];
            var muteEndTime = (DateTime) value[1];
            return muteEndTime > DateTime.Now ? "Unmute" : $"Mute for {timeSpan.ToReadableString()}";
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class FilePathToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterHelper.IsValueValid(value))
                return DependencyProperty.UnsetValue;
            var path = (string) value;
            return !File.Exists(path) ? null : Path.GetFileName(path);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class VisibilityToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterHelper.IsValueValid(value))
                return DependencyProperty.UnsetValue;
            return (Visibility) value == Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class NotCollapsedVisibilityToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterHelper.IsValueValid(value))
                return DependencyProperty.UnsetValue;
            return (Visibility) value != Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class NonAlwaysOnOpenModeToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterHelper.IsValueValid(value))
                return DependencyProperty.UnsetValue;
            return (OpenMode) value != OpenMode.AlwaysOpen;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TextFileTypeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterHelper.IsValueValid(value))
                return DependencyProperty.UnsetValue;
            return (FileType) value == FileType.Text ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ImageFileTypeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterHelper.IsValueValid(value))
                return DependencyProperty.UnsetValue;
            return (FileType) value == FileType.Image ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class OtherFileTypeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterHelper.IsValueValid(value))
                return DependencyProperty.UnsetValue;
            return (FileType) value == FileType.Other ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolToPauseTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterHelper.IsValueValid(value))
                return DependencyProperty.UnsetValue;
            return (bool) value ? "Resume" : "Pause";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DateTimeOffsetFormatToStringConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterHelper.IsValueValid(value, true))
                return DependencyProperty.UnsetValue;
            if (value.Length != 3)
                return DependencyProperty.UnsetValue;
            return ((DateTime) value[0] + (TimeSpan) value[2]).ToString((string) value[1]);
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class RSSFeedItemsConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterHelper.IsValueValid(value, true))
                return DependencyProperty.UnsetValue;
            if (value.Length != 5)
                return DependencyProperty.UnsetValue;
            var items = (ObservableCollection<FeedItem>) value[0];
            var max = (int) value[1];
            var titleWhitelist = (List<string>) value[2];
            var titleBlacklist = (List<string>) value[3];
            var categoryWhitelist = (List<string>) value[4];

            var newitems = items.Where(x => (categoryWhitelist == null || categoryWhitelist.Count == 0 ||
                                             categoryWhitelist.Any(y => x.Categories.Any(z => z.Name == y))) &&
                                            (titleWhitelist == null || titleWhitelist.Count == 0 ||
                                             titleWhitelist.Any(y => x.Title.Contains(y))) &&
                                            (titleBlacklist == null || titleBlacklist.Count == 0 ||
                                             titleBlacklist.All(y => !x.Title.Contains(y))));
            return max < 0 ? newitems : newitems.Take(max);
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StringEmptyToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.IsNullOrEmpty((string) value) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StringNotEmptyToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.IsNullOrEmpty((string) value) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ActionBarVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterHelper.IsValueValid(value, true))
                return DependencyProperty.UnsetValue;
            if (value.Length != 3)
                return DependencyProperty.UnsetValue;
            var visMode = (TitlebarVisibilityMode) value[0];
            var isMouseOver = (bool) value[1];
            var keepOpen = (bool) value[2];
            switch (visMode)
            {
                case TitlebarVisibilityMode.AlwaysVisible:
                    return Visibility.Visible;
                case TitlebarVisibilityMode.OnHover:
                    return isMouseOver || keepOpen ? Visibility.Visible : Visibility.Hidden;
                default:
                case TitlebarVisibilityMode.Hidden:
                    return Visibility.Collapsed;
            }
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class EventActionPairToNameConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.Length != 2)
                return DependencyProperty.UnsetValue;
            var pair = value[0] as EventActionPair;
            if (pair == null)
                return DependencyProperty.UnsetValue;
            var eventName = EventActionFactory.GetNameFromEvent(pair.Event);
            var actionName = EventActionFactory.GetNameFromAction(pair.Action);
            var name = pair.Name;
            var idName = $"{eventName} -> {actionName}";

            return string.IsNullOrWhiteSpace(name) ? idName : $"{name} ({idName})";
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DockIsVerticalToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dock = (Dock) value;
            return dock == Dock.Top || dock == Dock.Bottom;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DockIsHorizontalToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dock = (Dock) value;
            return dock == Dock.Left || dock == Dock.Right;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SelectedEventActionPairToEnableDisableNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterHelper.IsValueValid(value))
                return DependencyProperty.UnsetValue;
            var pair = value as EventActionPair;
            if (pair == null)
                return DependencyProperty.UnsetValue;
            return pair.Disabled ? "Enable" : "Disable";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SelectedWidgetToMuteUnmuteNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!ConverterHelper.IsValueValid(value))
                return DependencyProperty.UnsetValue;
            var settings = value as WidgetSettingsBase;
            if (settings == null)
                return DependencyProperty.UnsetValue;
            return settings.IsMuted() ? "Unmute" : "Mute";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}