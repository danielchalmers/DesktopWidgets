using DesktopWidgets.Helpers;

namespace DesktopWidgets.Actions
{
    public class WidgetDismissAction : WidgetActionBase
    {
        public override void ExecuteAction()
        {
            base.ExecuteAction();
            WidgetId?.GetView()?.Dismiss();
        }
    }
}