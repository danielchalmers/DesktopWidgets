using System;
using System.Windows;
using System.Windows.Media;

namespace DesktopWidgets.Classes
{
    public static class EnumUtilities
    {
        public static bool IsHorizontal(this ScreenDockPosition screenDockPosition)
        {
            return screenDockPosition == ScreenDockPosition.Left || screenDockPosition == ScreenDockPosition.Right;
        }

        public static bool IsVertical(this ScreenDockPosition screenDockPosition)
        {
            return screenDockPosition == ScreenDockPosition.Top || screenDockPosition == ScreenDockPosition.Bottom;
        }

        public static HorizontalAlignment ToHorizontalAlignment(this ShortcutAlignment shortcutAlignment)
        {
            switch (shortcutAlignment)
            {
                case ShortcutAlignment.Top:
                    return HorizontalAlignment.Left;
                default:
                case ShortcutAlignment.Center:
                    return HorizontalAlignment.Center;
                case ShortcutAlignment.Bottom:
                    return HorizontalAlignment.Right;
            }
        }

        public static VerticalAlignment ToVerticalAlignment(this ShortcutAlignment shortcutAlignment)
        {
            switch (shortcutAlignment)
            {
                case ShortcutAlignment.Top:
                    return VerticalAlignment.Top;
                default:
                case ShortcutAlignment.Center:
                    return VerticalAlignment.Center;
                case ShortcutAlignment.Bottom:
                    return VerticalAlignment.Bottom;
            }
        }

        public static System.Windows.Controls.ScrollBarVisibility ToWindowsScrollBarVisibility(
            this ScrollBarVisibility scrollBarVisibility)
        {
            switch (scrollBarVisibility)
            {
                default:
                case ScrollBarVisibility.Auto:
                    return System.Windows.Controls.ScrollBarVisibility.Auto;
                case ScrollBarVisibility.Visible:
                    return System.Windows.Controls.ScrollBarVisibility.Visible;
                case ScrollBarVisibility.Hidden:
                    return System.Windows.Controls.ScrollBarVisibility.Hidden;
            }
        }

        public static BitmapScalingMode ToBitmapScalingMode(this ImageScalingMode imageScalingMode)
        {
            switch (imageScalingMode)
            {
                case ImageScalingMode.HighQuality:
                    return BitmapScalingMode.HighQuality;
                default:
                case ImageScalingMode.LowQuality:
                    return BitmapScalingMode.LowQuality;
                case ImageScalingMode.NearestNeighbor:
                    return BitmapScalingMode.NearestNeighbor;
            }
        }

        public static double ConvertHorizontalPadding(this ScreenDockPosition screenDockPosition, ScreenDockAlignment
            screenDockAlignment, double padding)
        {
            if (screenDockPosition.IsHorizontal())
            {
                if (screenDockPosition == ScreenDockPosition.Left)
                {
                    return padding;
                }
                else if (screenDockPosition == ScreenDockPosition.Right)
                {
                    return -padding;
                }
            }
            else
            {
                if (screenDockAlignment == ScreenDockAlignment.Top)
                {
                    return padding;
                }
                else if (screenDockAlignment == ScreenDockAlignment.Bottom)
                {
                    return -padding;
                }
            }
            return 0;
        }

        public static double ConvertVerticalPadding(this ScreenDockPosition screenDockPosition,
            ScreenDockAlignment screenDockAlignment, double padding)
        {
            if (screenDockPosition.IsVertical())
            {
                if (screenDockPosition == ScreenDockPosition.Top)
                {
                    return padding;
                }
                else if (screenDockPosition == ScreenDockPosition.Bottom)
                {
                    return -padding;
                }
            }
            else
            {
                if (screenDockAlignment == ScreenDockAlignment.Top)
                {
                    return padding;
                }
                else if (screenDockAlignment == ScreenDockAlignment.Bottom)
                {
                    return -padding;
                }
            }
            return 0;
        }
    }
}