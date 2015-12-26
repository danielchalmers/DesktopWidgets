using System;
using System.IO;
using System.Threading;
using System.Windows.Media;

namespace DesktopWidgets.Helpers
{
    public static class SoundHelper
    {
        private static readonly MediaPlayer MediaPlayer = new MediaPlayer();

        public static void PlaySoundAsync(string uriPath, double volume = 1)
        {
            new Thread(delegate() { PlaySound(uriPath, volume); }).Start();
        }

        public static void PlaySound(string uriPath, double volume = 1)
        {
            if (string.IsNullOrWhiteSpace(uriPath) || !File.Exists(uriPath))
                return;
            MediaPlayer.Open(new Uri(uriPath));
            MediaPlayer.Volume = volume;
            MediaPlayer.Play();
        }
    }
}