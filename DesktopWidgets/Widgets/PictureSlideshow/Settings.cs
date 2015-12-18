using System;
using System.ComponentModel;
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

        [Category("General")]
        [DisplayName("Root Folder")]
        public string RootPath { get; set; }

        [Category("General")]
        [DisplayName("Allowed File Extensions")]
        public string FileFilterExtension { get; set; } = ".jpg|.jpeg|.png|.bmp|.gif|.ico|.tiff|.wmp";

        [Category("General")]
        [DisplayName("Maximum File Size")]
        public double FileFilterSize { get; set; } = 1024000;

        [Category("General")]
        [DisplayName("Next Image Interval")]
        public TimeSpan ChangeInterval { get; set; } = TimeSpan.FromSeconds(15);

        [Category("General")]
        [DisplayName("Shuffle")]
        public bool Shuffle { get; set; } = true;

        [Category("General")]
        [DisplayName("Recursive")]
        public bool Recursive { get; set; } = false;
    }
}