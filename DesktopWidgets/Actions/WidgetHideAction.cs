using System.ComponentModel;
using DesktopWidgets.Helpers;

namespace DesktopWidgets.Actions
{
    public class WidgetHideAction : WidgetActionBase
    {
        [DisplayName("Check Hide Status")]
        public bool CheckHideStatus { get; set; } = false;

        [DisplayName("Check Idle Status")]
        public bool CheckIdleStatus { get; set; } = false;

        public override void ExecuteAction()
        {
            base.ExecuteAction();
            WidgetId?.GetView()?.HideUi(CheckIdleStatus, CheckHideStatus);
        }
    }
}