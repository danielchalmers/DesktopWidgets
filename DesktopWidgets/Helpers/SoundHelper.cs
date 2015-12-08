using System;
using System.IO;
using System.Threading;
using System.Windows.Media;

namespace DesktopWidgets.Helpers
{
    public static class SoundHelper
    {
        public static void PlaySoundAsync(string uriPath, double volume = 1)
        {
            new Thread(delegate() { PlaySound(uriPath, volume); }).Start();
        }

        public static void PlaySound(string uriPath, double volume = 1)
        {
            if (string.IsNullOrWhiteSpace(uriPath) || !File.Exists(uriPath))
                return;
            var player = new MediaPlayer();
            player.Open(new Uri(uriPath));
            player.Volume = volume;
            player.Play();
        }
    }
}