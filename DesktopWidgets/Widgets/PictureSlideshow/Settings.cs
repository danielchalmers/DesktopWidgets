using System;
using System.ComponentModel;
using DesktopWidgets.WidgetBase.Settings;

namespace DesktopWidgets.Widgets.PictureSlideshow
{
    public class Settings : WidgetSettingsBase
    {
        public Settings()
        {
            Style.Width = 384;
            Style.Height = 216;
        }

        [Category("General")]
        [DisplayName("Image Folder Path")]
        public string RootPath { get; set; }

        [Category("General")]
        [DisplayName("Max File Size (bytes)")]
        public double FileFilterSize { get; set; } = 5242880;

        [Category("General")]
        [DisplayName("Next Image Interval")]
        public TimeSpan ChangeInterval { get; set; } = TimeSpan.FromSeconds(15);

        [Category("General")]
        [DisplayName("Shuffle")]
        public bool Shuffle { get; set; } = true;

        [Category("General")]
        [DisplayName("Recursive")]
        public bool Recursive { get; set; } = false;

        [Category("General")]
        [DisplayName("Current Image Path")]
        public string ImageUrl { get; set; }

        [Category("General")]
        [DisplayName("Allow Dropping Images")]
        public bool AllowDropFiles { get; set; } = true;

        [Category("General")]
        [DisplayName("Freeze")]
        public bool Freeze { get; set; }
    }
}