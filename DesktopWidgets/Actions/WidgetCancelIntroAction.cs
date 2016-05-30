using DesktopWidgets.Helpers;

namespace DesktopWidgets.Actions
{
    public class WidgetCancelIntroAction : WidgetActionBase
    {
        protected override void ExecuteAction()
        {
            base.ExecuteAction();
            WidgetId?.GetView()?.CancelIntro();
        }
    }
}