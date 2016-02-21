using DesktopWidgets.Helpers;

namespace DesktopWidgets.Actions
{
    public class WidgetDisableAction : WidgetActionBase
    {
        public override void ExecuteAction()
        {
            base.ExecuteAction();
            WidgetId?.Disable();
        }
    }
}