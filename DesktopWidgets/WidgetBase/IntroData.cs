namespace DesktopWidgets.WidgetBase
{
    public class IntroData
    {
        public IntroData(int duration = -1, bool reversable = false, bool activate = false,
            bool hideOnFinish = true, bool executeFinishAction = true)
        {
            Duration = duration;
            Reversable = reversable;
            Activate = activate;
            HideOnFinish = hideOnFinish;
            ExecuteFinishAction = executeFinishAction;
        }

        public int Duration { get; set; }
        public bool Reversable { get; set; }
        public bool Activate { get; set; }
        public bool HideOnFinish { get; set; }
        public bool ExecuteFinishAction { get; set; }
    }
}