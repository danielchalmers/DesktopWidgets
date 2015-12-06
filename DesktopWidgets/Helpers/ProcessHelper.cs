#region

using System.Diagnostics;
using System.IO;
using System.Threading;
using DesktopWidgets.Properties;

#endregion

namespace DesktopWidgets.Helpers
{
    internal class ProcessHelper
    {
        public static void Launch(string path, string args = "", string startIn = "",
            ProcessWindowStyle style = ProcessWindowStyle.Normal)
        {
            if (Settings.Default.LaunchProcessAsync)
                new Thread(delegate() { RunProcess(path, args, startIn, style); }).Start();
            else
                RunProcess(path, args, startIn, style);
        }

        private static void RunProcess(string path, string args, string startIn, ProcessWindowStyle style)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = path,
                Arguments = args,
                WorkingDirectory = startIn,
                WindowStyle = style
            });
        }

        public static void OpenFolder(string path)
        {
            if (File.Exists(path) || Directory.Exists(path))
                Launch("explorer.exe", "/select," + path);
        }
    }
}