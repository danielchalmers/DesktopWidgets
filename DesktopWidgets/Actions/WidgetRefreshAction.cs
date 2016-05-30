using DesktopWidgets.Helpers;

namespace DesktopWidgets.Actions
{
    public class WidgetRefreshAction : WidgetActionBase
    {
        public override void ExecuteAction()
        {
            base.ExecuteAction();
            WidgetId?.Refresh();
        }
    }
}