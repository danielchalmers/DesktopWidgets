using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace DesktopWidgets.WidgetBase.Styles
{
    [ExpandableObject]
    [DisplayName("Titlebar Style")]
    public class WidgetTitlebarStyle : BorderStyleBase
    {
        public WidgetTitlebarStyle()
        {
            BackgroundOpacity = 0.5;
            CornerRadius = new CornerRadius(4);
            BackgroundColor = Color.FromRgb(32, 32, 32);
            TextColor = Colors.WhiteSmoke;
        }

        [DisplayName("Visible")]
        public TitlebarVisibilityMode VisibilityMode { get; set; } = TitlebarVisibilityMode.AlwaysVisible;

        [DisplayName("Reload Button Visible")]
        public bool ShowReloadButton { get; set; } = false;

        [DisplayName("Menu Button Visible")]
        public bool ShowMenuButton { get; set; } = true;

        [DisplayName("Dismiss Button Visible")]
        public bool ShowDismissButton { get; set; } = false;

        [Category("Titlebar Name")]
        [DisplayName("Visible")]
        public bool ShowName { get; set; } = true;

        [Category("Titlebar Name")]
        [DisplayName("Horizontal Alignment")]
        public HorizontalAlignment NameHorizontalAlignment { get; set; } = HorizontalAlignment.Center;

        [Category("Titlebar Name")]
        [DisplayName("Allow Editing")]
        public bool NameAllowEditing { get; set; } = true;

        [DisplayName("Button Font Size")]
        public int ButtonFontSize { get; set; } = 12;

        [DisplayName("Button Size (px)")]
        public double ButtonSize { get; set; } = 16;
    }
}