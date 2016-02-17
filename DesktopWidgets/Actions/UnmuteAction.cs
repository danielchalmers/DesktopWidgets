namespace DesktopWidgets.Actions
{
    internal class UnmuteAction : IAction
    {
        public void Execute()
        {
            App.Unmute();
        }
    }
}