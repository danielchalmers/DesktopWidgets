using DesktopWidgets.Classes;
using DesktopWidgets.Stores;

namespace DesktopWidgets.Actions
{
    internal class PlaySoundAction : ActionBase
    {
        public SoundFile SoundFile { get; set; } = new SoundFile();

        public override void ExecuteAction()
        {
            base.ExecuteAction();
            MediaPlayerStore.PlaySoundAsync(SoundFile);
        }
    }
}