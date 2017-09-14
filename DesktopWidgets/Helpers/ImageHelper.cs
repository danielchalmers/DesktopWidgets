using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace DesktopWidgets.Helpers
{
    public static class ImageHelper
    {
        // https://msdn.microsoft.com/en-us/library/ee719654(v=VS.85).aspx#wpfc_codecs.
        public static readonly List<string> SupportedExtensions = new List<string>
        {
            ".bmp",
            ".gif",
            ".ico",
            ".jpg",
            ".jpeg",
            ".png",
            ".tiff",
            ".wmp",
            ".dds"
        };

        public static bool IsSupported(string extension)
        {
            return SupportedExtensions.Contains(extension.ToLower());
        }

        public static BitmapImage LoadBitmapImageFromPath(string path)
        {
            var bmi = new BitmapImage();
            bmi.BeginInit();
            bmi.CacheOption = BitmapCacheOption.OnLoad;
            bmi.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
            bmi.EndInit();
            bmi.Freeze();
            return bmi;
        }
    }
}