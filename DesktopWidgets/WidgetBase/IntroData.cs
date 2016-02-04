namespace DesktopWidgets.WidgetBase
{
    public class IntroData
    {
        public int Duration { get; set; } = -1;
        public bool Reversable { get; set; } = false;
        public bool Activate { get; set; } = false;
        public bool HideOnFinish { get; set; } = true;
        public bool ExecuteFinishAction { get; set; } = true;
        public string SoundPath { get; set; }
        public double SoundVolume { get; set; } = 1.0;
    }
}