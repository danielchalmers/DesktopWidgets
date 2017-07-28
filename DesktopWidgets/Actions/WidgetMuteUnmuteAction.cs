using System;
using System.ComponentModel;
using DesktopWidgets.Helpers;
using DesktopWidgets.Properties;

namespace DesktopWidgets.Actions
{
    public class WidgetMuteUnmuteAction : WidgetActionBase
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
                    WidgetId.ToggleMute(Duration);
                    break;
                case MuteMode.Mute:
                    WidgetId.Mute(Duration);
                    break;
                case MuteMode.Unmute:
                    WidgetId.Unmute();
                    break;
            }
        }
    }
}