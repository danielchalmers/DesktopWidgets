using DesktopWidgets.Helpers;

namespace DesktopWidgets.Actions
{
    public class WidgetHideAction : WidgetActionBase
    {
        public bool CheckHideStatus { get; set; } = false;
        public bool CheckIdleStatus { get; set; } = false;

        public override void Execute()
        {
            base.Execute();
            WidgetId.GetView()?.HideUi(CheckIdleStatus, CheckHideStatus);
        }
    }
}