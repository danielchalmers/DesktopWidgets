using DesktopWidgets.Classes;

namespace DesktopWidgets.Widgets.PictureFrame
{
    public class Settings : WidgetSettingsBase
    {
        public Settings()
        {
            Width = 384;
            Height = 216;
        }

        public string ImageUrl { get; set; }
    }
}