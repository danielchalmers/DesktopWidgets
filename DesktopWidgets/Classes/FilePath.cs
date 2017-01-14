using System.ComponentModel;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace DesktopWidgets.Classes
{
    [ExpandableObject]
    [DisplayName("File")]
    public class FilePath
    {
        public FilePath(string path)
        {
            Path = path;
        }

        public FilePath()
        {
        }

        [PropertyOrder(0)]
        [DisplayName("Path")]
        public string Path { get; set; } = "";

        public override string ToString()
        {
            return Path;
        }
    }
}