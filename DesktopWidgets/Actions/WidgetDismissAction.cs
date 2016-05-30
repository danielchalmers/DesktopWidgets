using DesktopWidgets.Helpers;

namespace DesktopWidgets.Actions
{
    public class WidgetDismissAction : WidgetActionBase
    {
        protected override void ExecuteAction()
        {
            base.ExecuteAction();
            WidgetId?.GetView()?.Dismiss();
        }
    }
}