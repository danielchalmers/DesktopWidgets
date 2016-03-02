using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DesktopWidgets.Classes;
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
        [DisplayName("Font Settings")]
        public FontSettings ButtonFontSettings { get; set; } = new FontSettings {FontSize = 16};

        [Category("Buttons")]
        [DisplayName("Size (px)")]
        public double ButtonSize { get; set; } = 24;

        [Category("Buttons")]
        [DisplayName("Margin")]
        public Thickness ButtonMargin { get; set; } = new Thickness(0, 2, 0, 2);

        [Category("Buttons")]
        [DisplayName("Dock")]
        public Dock ButtonDock { get; set; } = Dock.Bottom;

        [Category("Buttons")]
        [DisplayName("Menu Visible")]
        public bool ShowMenuButton { get; set; } = true;

        [Category("Buttons")]
        [DisplayName("Reload Visible")]
        public bool ShowReloadButton { get; set; } = true;

        [Category("Buttons")]
        [DisplayName("Dismiss Visible")]
        public bool ShowDismissButton { get; set; } = true;

        [Category("Name")]
        [DisplayName("Visible")]
        public bool ShowName { get; set; } = false;

        [Category("Name")]
        [DisplayName("Dock")]
        public Dock NameDock { get; set; } = Dock.Top;

        [DisplayName("Stay Open Duration (ms)")]
        public int StayOpenDuration { get; set; } = 1500;
    }
}