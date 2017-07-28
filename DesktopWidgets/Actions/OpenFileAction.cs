using DesktopWidgets.Classes;
using DesktopWidgets.Helpers;

namespace DesktopWidgets.Actions
{
    public class OpenFileAction : ActionBase
    {
        public ProcessFile ProcessFile { get; set; } = new ProcessFile();

        protected override void ExecuteAction()
        {
            base.ExecuteAction();
            ProcessHelper.Launch(ProcessFile);
        }
    }
}