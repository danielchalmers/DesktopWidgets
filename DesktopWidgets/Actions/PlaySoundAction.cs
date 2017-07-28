using DesktopWidgets.Classes;
using DesktopWidgets.Stores;

namespace DesktopWidgets.Actions
{
    public class PlaySoundAction : ActionBase
    {
        public SoundFile SoundFile { get; set; } = new SoundFile();

        protected override void ExecuteAction()
        {
            base.ExecuteAction();
            MediaPlayerStore.PlaySoundAsync(SoundFile);
        }
    }
}