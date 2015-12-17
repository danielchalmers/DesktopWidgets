#region

using DesktopWidgets.Classes;

#endregion

namespace DesktopWidgets.Helpers
{
    internal static class Win32Helper
    {
        public static Win32App GetForegroundApp() => new Win32App(NativeMethods.GetForegroundWindow());
    }
}