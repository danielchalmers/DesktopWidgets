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

        [DisplayName("Path")]
        public string Path { get; set; }
    }
}