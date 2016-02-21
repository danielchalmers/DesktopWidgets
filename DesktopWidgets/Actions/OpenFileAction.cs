using DesktopWidgets.Classes;
using DesktopWidgets.Helpers;

namespace DesktopWidgets.Actions
{
    internal class OpenFileAction : ActionBase
    {
        public ProcessFile ProcessFile { get; set; } = new ProcessFile();

        public override void ExecuteAction()
        {
            base.ExecuteAction();
            ProcessHelper.Launch(ProcessFile);
        }
    }
}