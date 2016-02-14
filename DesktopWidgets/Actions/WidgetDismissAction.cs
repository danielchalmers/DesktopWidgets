using DesktopWidgets.Helpers;

namespace DesktopWidgets.Actions
{
    public class WidgetDismissAction : WidgetActionBase
    {
        public override void Execute()
        {
            base.Execute();
            WidgetId?.GetView()?.Dismiss();
        }
    }
}