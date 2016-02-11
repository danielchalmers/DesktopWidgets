using DesktopWidgets.Helpers;
using DesktopWidgets.WidgetBase;

namespace DesktopWidgets.Actions
{
    public class WidgetIntroAction : WidgetActionBase
    {
        public IntroData IntroSettings { get; set; }

        public override void Execute()
        {
            base.Execute();
            WidgetId.GetView()?.ShowIntro(IntroSettings);
        }
    }
}