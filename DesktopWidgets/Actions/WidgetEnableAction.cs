using DesktopWidgets.Helpers;

namespace DesktopWidgets.Actions
{
    public class WidgetEnableAction : WidgetActionBase
    {
        protected override void ExecuteAction()
        {
            base.ExecuteAction();
            WidgetId?.Enable();
        }
    }
}