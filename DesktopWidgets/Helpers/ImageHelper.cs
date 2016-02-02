using System.Collections.Generic;

namespace DesktopWidgets.Helpers
{
    public static class ImageHelper
    {
        public static readonly List<string> SupportedExtensions = new List<string>
        {
            ".bmp",
            ".gif",
            ".ico",
            ".jpg",
            ".jpeg",
            ".png",
            ".tiff"
        };

        public static bool IsSupported(string extension)
        {
            return SupportedExtensions.Contains(extension.ToLower());
        }
    }
}