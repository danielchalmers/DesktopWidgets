using DesktopWidgets.Classes;
using DesktopWidgets.Helpers;

namespace DesktopWidgets.Actions
{
    public class WidgetIntroAction : WidgetActionBase
    {
        public IntroData IntroSettings { get; set; } = new IntroData();

        protected override void ExecuteAction()
        {
            base.ExecuteAction();
            WidgetId?.GetView()?.ShowIntro(IntroSettings);
        }
    }
}