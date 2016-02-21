using DesktopWidgets.Helpers;

namespace DesktopWidgets.Actions
{
    public class WidgetCancelIntroAction : WidgetActionBase
    {
        public override void ExecuteAction()
        {
            base.ExecuteAction();
            WidgetId?.GetView()?.CancelIntro();
        }
    }
}