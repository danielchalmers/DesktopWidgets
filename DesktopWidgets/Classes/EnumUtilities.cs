using System.Windows.Media;

namespace DesktopWidgets.Classes
{
    public static class EnumUtilities
    {
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
    }
}