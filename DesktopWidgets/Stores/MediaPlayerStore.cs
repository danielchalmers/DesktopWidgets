using System.Collections.Generic;
using System.IO;
using System.Threading;
using DesktopWidgets.Classes;
using DesktopWidgets.Properties;
using WMPLib;

namespace DesktopWidgets.Stores
{
    public static class MediaPlayerStore
    {
        private static readonly List<WindowsMediaPlayer> MediaPlayers = new List<WindowsMediaPlayer>();

        private static WindowsMediaPlayer GetAvailablePlayer()
        {
            try
            {
                for (var i = 0; i < Settings.Default.MaxConcurrentMediaPlayers; i++)
                {
                    if (MediaPlayers.Count - 1 >= i && MediaPlayers[i] == null)
                        MediaPlayers.RemoveAt(i);
                    if (MediaPlayers.Count - 1 < i)
                        MediaPlayers.Add(new WindowsMediaPlayer());
                    if (MediaPlayers[i].playState != WMPPlayState.wmppsPlaying)
                        return MediaPlayers[i];
                }
            }
            catch
            {
                // ignored
            }
            if (MediaPlayers.Count == 0)
                MediaPlayers.Add(new WindowsMediaPlayer());
            return MediaPlayers[0];
        }

        public static void PlaySoundAsync(SoundFile soundFile)
        {
            new Thread(delegate() { Play(soundFile); }).Start();
        }

        public static void PlaySoundAsync(string path, double volume = 1)
        {
            new Thread(delegate() { Play(path, volume); }).Start();
        }

        public static void Play(SoundFile soundFile) => Play(soundFile.Path, soundFile.Volume);

        public static void Play(string path, double volume = 1)
        {
            if (App.IsMuted || string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                return;
            var player = GetAvailablePlayer();
            player.settings.volume = (int) (volume*100);
            player.URL = path;
        }

        public static void StopAll()
        {
            foreach (var player in MediaPlayers)
                player.close();
        }
    }
}