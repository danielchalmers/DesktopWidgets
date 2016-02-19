using DesktopWidgets.Classes;
using DesktopWidgets.Helpers;

namespace DesktopWidgets.Actions
{
    internal class OpenFileAction : IAction
    {
        public ProcessFile ProcessFile { get; set; } = new ProcessFile();

        public void Execute()
        {
            ProcessHelper.Launch(ProcessFile);
        }
    }
}