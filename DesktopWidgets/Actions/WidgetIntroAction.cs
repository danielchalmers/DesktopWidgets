using DesktopWidgets.Classes;
using DesktopWidgets.Helpers;

namespace DesktopWidgets.Actions
{
    public class WidgetIntroAction : WidgetActionBase
    {
        public IntroData IntroSettings { get; set; } = new IntroData();

        public override void Execute()
        {
            base.Execute();
            WidgetId.GetView()?.ShowIntro(IntroSettings);
        }
    }
}