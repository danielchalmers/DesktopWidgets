using System.ComponentModel;
using System.Diagnostics;
using DesktopWidgets.Helpers;

namespace DesktopWidgets.Actions
{
    internal class OpenFileAction : IAction
    {
        [DisplayName("Path")]
        public string Path { get; set; }

        [DisplayName("Arguments")]
        public string Arguments { get; set; }

        [DisplayName("Start In Folder")]
        public string StartInFolder { get; set; }

        [DisplayName("Window Style")]
        public ProcessWindowStyle WindowStyle { get; set; } = ProcessWindowStyle.Normal;

        public void Execute()
        {
            ProcessHelper.Launch(Path, Arguments, StartInFolder, WindowStyle);
        }
    }
}