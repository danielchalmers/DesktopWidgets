using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace DesktopWidgets.WidgetBase.Styles
{
    [ExpandableObject]
    [DisplayName("Action Bar Style")]
    public class WidgetActionBarStyle : BorderStyleBase
    {
        public WidgetActionBarStyle()
        {
            BackgroundOpacity = 0.5;
            CornerRadius = new CornerRadius(4);
            BackgroundColor = Color.FromRgb(32, 32, 32);
            TextColor = Colors.WhiteSmoke;
            HorizontalAlignment = HorizontalAlignment.Center;
            VerticalAlignment = VerticalAlignment.Top;
        }

        [DisplayName("Visibility")]
        public TitlebarVisibilityMode VisibilityMode { get; set; } = TitlebarVisibilityMode.OnHover;

        [DisplayName("Dock")]
        public Dock Dock { get; set; } = Dock.Right;

        [DisplayName("Margin")]
        public Thickness Margin { get; set; } = new Thickness(10, 0, 0, 0);

        [DisplayName("Orientation")]
        public Orientation Orientation { get; set; } = Orientation.Vertical;

        [Category("Buttons")]
        [DisplayName("Button Font Size")]
        public int ButtonFontSize { get; set; } = 16;

        [Category("Buttons")]
        [DisplayName("Button Size (px)")]
        public double ButtonSize { get; set; } = 24;

        [Category("Buttons")]
        [DisplayName("Button Margin")]
        public Thickness ButtonMargin { get; set; } = new Thickness(0, 2, 0, 2);

        [Category("Name")]
        [DisplayName("Show Name")]
        public bool ShowName { get; set; } = false;

        [DisplayName("Stay Open Duration (ms)")]
        public int StayOpenDuration { get; set; } = 1500;
    }
}