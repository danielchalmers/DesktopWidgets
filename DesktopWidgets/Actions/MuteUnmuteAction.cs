using System;
using System.ComponentModel;
using DesktopWidgets.Properties;

namespace DesktopWidgets.Actions
{
    public class MuteUnmuteAction : ActionBase
    {
        [DisplayName("Duration")]
        public TimeSpan Duration { get; set; } = Settings.Default.MuteDuration;

        [DisplayName("Mode")]
        public MuteMode Mode { get; set; } = MuteMode.Toggle;

        protected override void ExecuteAction()
        {
            base.ExecuteAction();
            switch (Mode)
            {
                case MuteMode.Toggle:
                    App.ToggleMute(Duration);
                    break;
                case MuteMode.Mute:
                    App.Mute(Duration);
                    break;
                case MuteMode.Unmute:
                    App.Unmute();
                    break;
            }
        }
    }
}