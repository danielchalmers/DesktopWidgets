using System;
using System.ComponentModel;
using DesktopWidgets.Properties;

namespace DesktopWidgets.Actions
{
    internal class MuteAction : IAction
    {
        [DisplayName("Duration")]
        public TimeSpan Duration { get; set; } = Settings.Default.MuteDuration;

        [DisplayName("Toggle")]
        public bool Toggle { get; set; } = false;

        public void Execute()
        {
            if (Toggle)
                App.ToggleMute();
            else
                App.Mute();
        }
    }
}