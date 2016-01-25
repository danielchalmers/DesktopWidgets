using System.Collections.Generic;

namespace DesktopWidgets.Helpers
{
    public static class MediaPlayerHelper
    {
        // List of formats supported by "Windows Media Player 12" - https://support.microsoft.com/en-us/kb/316992.
        private static readonly List<string> SupportedExtensions = new List<string>
        {
            ".asf",
            ".wma",
            ".wmv",
            ".wm",
            ".asx",
            ".wax",
            ".wvx",
            ".wmx",
            ".wpl",
            ".dvr-ms",
            ".wmd",
            ".avi",
            ".mpg",
            ".mpeg",
            ".m1v",
            ".mp2",
            ".mp3",
            ".mpa",
            ".mpe",
            ".m3u",
            ".mid",
            ".midi",
            ".rmi",
            ".aif",
            ".aifc",
            ".aiff",
            ".au",
            ".snd",
            ".wav",
            ".cda",
            ".ivf",
            ".wmz",
            ".wms",
            ".mov",
            ".m4a",
            ".mp4",
            ".m4v",
            ".mp4v",
            ".3g2",
            ".3gp2",
            ".3gp",
            ".3gpp",
            ".aac",
            ".adt",
            ".adts",
            ".m2ts"
        };

        public static bool IsSupported(string extension)
        {
            return SupportedExtensions.Contains(extension.ToLower());
        }
    }
}