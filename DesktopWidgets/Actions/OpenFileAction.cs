using System.Diagnostics;
using DesktopWidgets.Helpers;

namespace DesktopWidgets.Actions
{
    internal class OpenFileAction : IAction
    {
        public string Path { get; set; }
        public string Arguments { get; set; }
        public string StartInFolder { get; set; }
        public ProcessWindowStyle WindowStyle { get; set; } = ProcessWindowStyle.Normal;

        public void Execute()
        {
            ProcessHelper.Launch(Path, Arguments, StartInFolder, WindowStyle);
        }
    }
}