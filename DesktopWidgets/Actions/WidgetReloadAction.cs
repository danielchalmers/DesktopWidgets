using DesktopWidgets.Helpers;

namespace DesktopWidgets.Actions
{
    public class WidgetReloadAction : WidgetActionBase
    {
        public override void Execute()
        {
            base.Execute();
            WidgetId?.Reload();
        }
    }
}