using DesktopWidgets.Helpers;

namespace DesktopWidgets.Actions
{
    public class WidgetReloadAction : WidgetActionBase
    {
        public override void ExecuteAction()
        {
            base.ExecuteAction();
            WidgetId?.Reload();
        }
    }
}