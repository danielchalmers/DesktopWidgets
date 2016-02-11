using DesktopWidgets.Helpers;

namespace DesktopWidgets.Actions
{
    public class WidgetEnableAction : WidgetActionBase
    {
        public override void Execute()
        {
            base.Execute();
            WidgetId.Enable();
        }
    }
}