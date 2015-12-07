using System;
using DesktopWidgets.Classes;

namespace DesktopWidgets.Widgets.PictureSlideshow
{
    public class Settings : WidgetSettingsBase
    {
        public Settings()
        {
            Width = 384;
            Height = 216;
        }

        public string RootPath { get; set; }
        public string FileFilterExtension { get; set; } = ".jpg|.jpeg|.png|.bmp|.gif|.ico|.tiff|.wmp";
        public double FileFilterSize { get; set; } = 1024000;
        public TimeSpan ChangeInterval { get; set; } = TimeSpan.FromSeconds(15);
        public bool Shuffle { get; set; } = true;
        public bool Recursive { get; set; } = false;
    }
}