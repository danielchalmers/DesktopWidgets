using System.ComponentModel;
using DesktopWidgets.WidgetBase.Settings;

namespace DesktopWidgets.Widgets.TimeClock
{
    public class Settings : WidgetClockSettingsBase
    {
        [Category("Behavior")]
        [DisplayName("Copy Time On Double Click")]
        public bool CopyTextOnDoubleClick { get; set; }

        public override void SetDefaults()
        {
            base.SetDefaults();
            CopyTextOnDoubleClick = true;
        }
    }
}