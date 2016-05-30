using DesktopWidgets.Helpers;

namespace DesktopWidgets.Actions
{
    public class WidgetSpecialAction : WidgetActionBase
    {
        protected override void ExecuteAction()
        {
            base.ExecuteAction();
            WidgetId?.GetView()?.ExecuteSpecialAction();
        }
    }
}