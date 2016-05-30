using DesktopWidgets.Helpers;

namespace DesktopWidgets.Actions
{
    public class WidgetRefreshAction : WidgetActionBase
    {
        protected override void ExecuteAction()
        {
            base.ExecuteAction();
            WidgetId?.Refresh();
        }
    }
}