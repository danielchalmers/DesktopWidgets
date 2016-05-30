using DesktopWidgets.Helpers;

namespace DesktopWidgets.Actions
{
    public class WidgetReloadAction : WidgetActionBase
    {
        protected override void ExecuteAction()
        {
            base.ExecuteAction();
            WidgetId?.Reload();
        }
    }
}