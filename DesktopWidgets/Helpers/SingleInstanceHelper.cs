using System;
using DesktopWidgets.Classes;

namespace DesktopWidgets.Helpers
{
    public static class SingleInstanceHelper
    {
        private const int HWND_BROADCAST = 0xffff;

        public static readonly int WM_SHOWAPP = NativeMethods.RegisterWindowMessage($"WM_SHOWAPP|{AssemblyInfo.Guid}");


        public static void ShowFirstInstance()
        {
            NativeMethods.SendNotifyMessage(
                (IntPtr)HWND_BROADCAST,
                (uint)WM_SHOWAPP,
                UIntPtr.Zero,
                IntPtr.Zero);
        }
    }
}