using System.ComponentModel;

namespace DesktopWidgets.Classes
{
    [DisplayName("Sound File")]
    public class SoundFile : FilePath
    {
        public SoundFile(string path, double volume) : base(path)
        {
            Path = path;
            Volume = volume;
        }

        public SoundFile()
        {
        }

        [DisplayName("Volume")]
        public double Volume { get; set; } = 1.0;
    }
}