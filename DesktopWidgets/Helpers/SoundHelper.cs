using System.IO;
using System.Threading;
using WMPLib;

namespace DesktopWidgets.Helpers
{
    public static class SoundHelper
    {
        private static readonly WindowsMediaPlayer MediaPlayer = new WindowsMediaPlayer();

        public static void PlaySoundAsync(string uriPath, double volume = 1)
        {
            new Thread(delegate() { PlaySound(uriPath, volume); }).Start();
        }

        public static void PlaySound(string uriPath, double volume = 1)
        {
            if (string.IsNullOrWhiteSpace(uriPath) || !File.Exists(uriPath))
                return;
            MediaPlayer.settings.volume = (int) (volume*100);
            MediaPlayer.URL = uriPath;
        }
    }
}