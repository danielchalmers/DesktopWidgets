using System;
using System.IO;
using System.Windows.Media;

namespace DesktopWidgets.Helpers
{
    public static class SoundHelper
    {
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