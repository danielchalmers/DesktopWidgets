using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace DesktopWidgets.WidgetBase.Styles
{
    [ExpandableObject]
    [DisplayName("Style")]
    public class WidgetStyle : BorderStyleBase
    {
        public WidgetStyle()
        {
            FontSettings.FontSize = 14;
            Padding = new Thickness(5);
            TextColor = Colors.Black;
            BackgroundColor = Colors.White;
            BackgroundOpacity = 0.95;
            Width = double.NaN;
            Height = double.NaN;
            CornerRadius = new CornerRadius(4);
            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;
        }

        [Category("Size")]
        [DisplayName("Minimum Width (px)")]
        public double MinWidth { get; set; } = double.NaN;

        [Category("Size")]
        [DisplayName("Minimum Height (px)")]
        public double MinHeight { get; set; } = double.NaN;

        [Category("Size")]
        [DisplayName("Maximum Width (px)")]
        public double MaxWidth { get; set; } = double.NaN;

        [Category("Size")]
        [DisplayName("Maximum Height (px)")]
        public double MaxHeight { get; set; } = double.NaN;

        [Category("Animation")]
        [DisplayName("Ease Animation")]
        public bool AnimationEase { get; set; } = true;

        [Category("Animation")]
        [DisplayName("Animation Type")]
        public AnimationType AnimationType { get; set; } = AnimationType.Fade;

        [Category("Animation")]
        [DisplayName("Animation Duration (ms)")]
        public int AnimationTime { get; set; } = 250;

        [Category("Background Image")]
        [DisplayName("Path")]
        public string BackgroundImagePath { get; set; }

        [Category("Background Image")]
        [DisplayName("Opacity")]
        public double BackgroundImageOpacity { get; set; } = 1.0;

        [DisplayName("Context Menu Enabled")]
        public bool ShowContextMenu { get; set; } = true;

        [DisplayName("Scrollbar Visibility")]
        public ScrollBarVisibility ScrollBarVisibility { get; set; } = ScrollBarVisibility.Auto;

        [Category("Frame")]
        [DisplayName("Enable Top Frame")]
        public bool ShowTopFrame { get; set; } = true;

        [Category("Frame")]
        [DisplayName("Enable Bottom Frame")]
        public bool ShowBottomFrame { get; set; } = true;

        [Category("Frame")]
        [DisplayName("Enable Left Frame")]
        public bool ShowLeftFrame { get; set; } = true;

        [Category("Frame")]
        [DisplayName("Enable Right Frame")]
        public bool ShowRightFrame { get; set; } = true;

        [Category("Border")]
        [DisplayName("Visible")]
        public bool BorderEnabled { get; set; } = true;

        [Category("Border")]
        [DisplayName("Border Color")]
        public Color BorderColor { get; set; } = Colors.Gray;

        [Category("Border")]
        [DisplayName("Border Opacity")]
        public double BorderOpacity { get; set; } = 0.5;

        [Category("Border")]
        [DisplayName("Thickness")]
        public Thickness BorderThickness { get; set; } = new Thickness(1);
    }
}