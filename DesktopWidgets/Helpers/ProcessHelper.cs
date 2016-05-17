#region

using System.Diagnostics;
using System.IO;
using DesktopWidgets.Classes;

#endregion

namespace DesktopWidgets.Helpers
{
    internal static class ProcessHelper
    {
        public static void Launch(string path, string args = "", string startIn = "",
            ProcessWindowStyle style = ProcessWindowStyle.Normal)
        {
            Launch(new ProcessFile {Path = path, Arguments = args, StartInFolder = startIn, WindowStyle = style});
        }

        public static void Launch(ProcessFile file)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = file.Path,
                Arguments = file.Arguments,
                WorkingDirectory = file.StartInFolder,
                WindowStyle = file.WindowStyle
            });
        }

        public static void OpenFolder(string path)
        {
            if (File.Exists(path) || Directory.Exists(path))
                Launch("explorer.exe", "/select," + path);
        }
    }
}