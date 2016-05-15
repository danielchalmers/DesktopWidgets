using System.ComponentModel;
using System.Diagnostics;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace DesktopWidgets.Classes
{
    [ExpandableObject]
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

        [PropertyOrder(2)]
        [DisplayName("Arguments")]
        public string Arguments { get; set; } = "";

        [PropertyOrder(1)]
        [DisplayName("Start in Folder")]
        public string StartInFolder { get; set; } = "";

        [PropertyOrder(3)]
        [DisplayName("Window Style")]
        public ProcessWindowStyle WindowStyle { get; set; } = ProcessWindowStyle.Normal;
    }
}