using System;
using System.ComponentModel;
using DesktopWidgets.WidgetBase.Settings;

namespace DesktopWidgets.Widgets.PictureSlideshow
{
    public class Settings : WidgetSettingsBase
    {
        [Category("General")]
        [DisplayName("Image Folder Path")]
        public string RootPath { get; set; }

        [Category("General")]
        [DisplayName("Maximum File Size (bytes)")]
        public double FileFilterSize { get; set; }

        [Category("General")]
        [DisplayName("Next Image Interval")]
        public TimeSpan ChangeInterval { get; set; }

        [Category("General")]
        [DisplayName("Shuffle")]
        public bool Shuffle { get; set; }

        [Category("General")]
        [DisplayName("Recursive")]
        public bool Recursive { get; set; }

        [Category("General")]
        [DisplayName("Current Image Path")]
        public string ImageUrl { get; set; }

        [Category("General")]
        [DisplayName("Allow Dropping Images")]
        public bool AllowDropFiles { get; set; }

        [Category("General")]
        [DisplayName("Freeze")]
        public bool Freeze { get; set; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            RootPath = "";
            FileFilterSize = 1024000;
            ChangeInterval = TimeSpan.FromSeconds(15);
            Shuffle = true;
            Recursive = false;
            ImageUrl = "";
            AllowDropFiles = true;
            Freeze = false;
            Width = 384;
            Height = 216;
        }
    }
}