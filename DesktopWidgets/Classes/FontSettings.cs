using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace DesktopWidgets.Classes
{
    [ExpandableObject]
    [DisplayName("Font Settings")]
    public class FontSettings
    {
        [DisplayName("Font Family")]
        public FontFamily FontFamily { get; set; } = new FontFamily("Segoe UI");

        [DisplayName("Font Size")]
        public int FontSize { get; set; } = 12;

        [DisplayName("Font Weight")]
        public FontWeight FontWeight { get; set; } = FontWeights.Normal;
    }
}