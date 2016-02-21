using DesktopWidgets.Helpers;

namespace DesktopWidgets.Actions
{
    public class WidgetEnableAction : WidgetActionBase
    {
        public override void ExecuteAction()
        {
            base.ExecuteAction();
            WidgetId?.Enable();
        }
    }
}