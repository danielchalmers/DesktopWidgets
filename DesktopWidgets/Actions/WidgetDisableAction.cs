using DesktopWidgets.Helpers;

namespace DesktopWidgets.Actions
{
    public class WidgetDisableAction : WidgetActionBase
    {
        protected override void ExecuteAction()
        {
            base.ExecuteAction();
            WidgetId?.Disable();
        }
    }
}