using DesktopWidgets.Stores;

namespace DesktopWidgets.Actions
{
    internal class PlaySoundAction : IAction
    {
        public string Path { get; set; }
        public double Volume { get; set; } = 1.0;

        public void Execute()
        {
            MediaPlayerStore.PlaySoundAsync(Path, Volume);
        }
    }
}