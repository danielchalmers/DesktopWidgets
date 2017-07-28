using DesktopWidgets.Classes;

namespace DesktopWidgets.Helpers
{
    public static class Win32Helper
    {
        public static Win32App GetForegroundApp() => new Win32App(NativeMethods.GetForegroundWindow());
    }
}