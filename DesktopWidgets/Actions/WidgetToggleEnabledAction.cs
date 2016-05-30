using DesktopWidgets.Helpers;

namespace DesktopWidgets.Actions
{
    public class WidgetToggleEnabledAction : WidgetActionBase
    {
        protected override void ExecuteAction()
        {
            base.ExecuteAction();
            WidgetId?.ToggleEnable();
        }
    }
}