using System.ComponentModel;
using System.Diagnostics;

namespace DesktopWidgets.Classes
{
    [DisplayName("Process File")]
    public class ProcessFile : FilePath
    {
        public ProcessFile(string path) : base(path)
        {
            Path = path;
        }

        public ProcessFile()
        {
        }

        [DisplayName("Arguments")]
        public string Arguments { get; set; } = "";

        [DisplayName("Start in Folder")]
        public string StartInFolder { get; set; } = "";

        [DisplayName("Window Style")]
        public ProcessWindowStyle WindowStyle { get; set; } = ProcessWindowStyle.Normal;
    }
}