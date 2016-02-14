using DesktopWidgets.Helpers;

namespace DesktopWidgets.Actions
{
    public class WidgetCancelIntroAction : WidgetActionBase
    {
        public override void Execute()
        {
            base.Execute();
            WidgetId?.GetView()?.CancelIntro();
        }
    }
}