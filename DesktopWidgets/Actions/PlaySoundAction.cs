using DesktopWidgets.Classes;
using DesktopWidgets.Stores;

namespace DesktopWidgets.Actions
{
    internal class PlaySoundAction : IAction
    {
        public SoundFile SoundFile { get; set; } = new SoundFile();

        public void Execute()
        {
            MediaPlayerStore.PlaySoundAsync(SoundFile);
        }
    }
}