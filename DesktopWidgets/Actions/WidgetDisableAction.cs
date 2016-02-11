using DesktopWidgets.Helpers;

namespace DesktopWidgets.Actions
{
    public class WidgetDisableAction : WidgetActionBase
    {
        public override void Execute()
        {
            base.Execute();
            WidgetId.Disable();
        }
    }
}