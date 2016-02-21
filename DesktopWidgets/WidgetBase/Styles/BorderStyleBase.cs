using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using DesktopWidgets.Classes;

namespace DesktopWidgets.WidgetBase.Styles
{
    public class BorderStyleBase
    {
        [DisplayName("Padding")]
        public Thickness Padding { get; set; }

        [Category("Text")]
        [DisplayName("Color")]
        public Color TextColor { get; set; }

        [Category("Background")]
        [DisplayName("Color")]
        public Color BackgroundColor { get; set; }

        [Category("Background")]
        [DisplayName("Opacity")]
        public double BackgroundOpacity { get; set; }

        [Category("Size")]
        [DisplayName("Width (px)")]
        public double Width { get; set; } = double.NaN;

        [Category("Size")]
        [DisplayName("Height (px)")]
        public double Height { get; set; } = double.NaN;

        [DisplayName("Corner Radius")]
        public CornerRadius CornerRadius { get; set; } = new CornerRadius(4);

        [Category("Text")]
        public FontSettings FontSettings { get; set; } = new FontSettings();

        [DisplayName("Horizontal Alignment")]
        public HorizontalAlignment HorizontalAlignment { get; set; }

        [DisplayName("Vertical Alignment")]
        public VerticalAlignment VerticalAlignment { get; set; }
    }
}