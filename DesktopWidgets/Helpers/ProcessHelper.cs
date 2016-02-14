#region

using System.Diagnostics;
using System.IO;
using System.Threading;
using DesktopWidgets.Classes;
using DesktopWidgets.Properties;

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
            if (Settings.Default.LaunchProcessAsync)
                new Thread(delegate() { RunProcess(file); }).Start();
            else
                RunProcess(file);
        }

        private static void RunProcess(ProcessFile file)
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