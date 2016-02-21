namespace DesktopWidgets.Actions
{
    internal class UnmuteAction : ActionBase
    {
        public override void ExecuteAction()
        {
            base.ExecuteAction();
            App.Unmute();
        }
    }
}