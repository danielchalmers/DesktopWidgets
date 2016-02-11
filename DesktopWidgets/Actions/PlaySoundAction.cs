using System.ComponentModel;
using DesktopWidgets.Stores;

namespace DesktopWidgets.Actions
{
    internal class PlaySoundAction : IAction
    {
        [DisplayName("Path")]
        public string Path { get; set; }

        [DisplayName("Volume")]
        public double Volume { get; set; } = 1.0;

        public void Execute()
        {
            MediaPlayerStore.PlaySoundAsync(Path, Volume);
        }
    }
}