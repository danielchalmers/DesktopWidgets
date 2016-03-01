using DesktopWidgets.Helpers;

namespace DesktopWidgets.Actions
{
    public class WidgetSpecialAction : WidgetActionBase
    {
        public override void ExecuteAction()
        {
            base.ExecuteAction();
            WidgetId?.GetView()?.ExecuteSpecialAction();
        }
    }
}