using DesktopWidgets.Helpers;

namespace DesktopWidgets.Actions
{
    public class WidgetToggleEnabledAction : WidgetActionBase
    {
        public override void Execute()
        {
            base.Execute();
            WidgetId?.ToggleEnable();
        }
    }
}