using System.Collections.Generic;
using System.IO;
using System.Threading;
using DesktopWidgets.Properties;
using WMPLib;

namespace DesktopWidgets.Classes
{
    public static class MediaPlayerStore
    {
        private static readonly List<WindowsMediaPlayer> _mediaPlayerList = new List<WindowsMediaPlayer>();

        private static WindowsMediaPlayer GetAvailablePlayer()
        {
            try
            {
                for (var i = 0; i < Settings.Default.MaxConcurrentMediaPlayers; i++)
                {
                    if (_mediaPlayerList.Count - 1 < i)
                        _mediaPlayerList.Add(new WindowsMediaPlayer());
                    if (_mediaPlayerList[i].playState != WMPPlayState.wmppsPlaying)
                        return _mediaPlayerList[i];
                }
            }
            catch
            {
                // ignored
            }
            return _mediaPlayerList[0];
        }

        public static void PlaySoundAsync(string path, double volume = 1)
        {
            new Thread(delegate() { Play(path, volume); }).Start();
        }

        public static void Play(string path, double volume = 1)
        {
            var player = GetAvailablePlayer();
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                return;
            player.settings.volume = (int) (volume*100);
            player.URL = path;
        }
    }
}