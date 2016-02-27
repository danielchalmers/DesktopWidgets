using System;
using System.ComponentModel;
using DesktopWidgets.Properties;

namespace DesktopWidgets.Actions
{
    internal class MuteAction : ActionBase
    {
        [DisplayName("Duration")]
        public TimeSpan Duration { get; set; } = Settings.Default.MuteDuration;

        [DisplayName("Toggle")]
        public bool Toggle { get; set; } = false;

        public override void ExecuteAction()
        {
            base.ExecuteAction();
            if (Toggle)
                App.ToggleMute(Duration);
            else
                App.Mute(Duration);
        }
    }
}