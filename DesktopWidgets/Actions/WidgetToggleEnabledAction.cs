using DesktopWidgets.Helpers;

namespace DesktopWidgets.Actions
{
    public class WidgetToggleEnabledAction : WidgetActionBase
    {
        public override void ExecuteAction()
        {
            base.ExecuteAction();
            WidgetId?.ToggleEnable();
        }
    }
}