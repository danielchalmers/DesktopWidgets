using System;
using System.Runtime.InteropServices;

namespace DesktopWidgets.Classes
{
    public static class SingleInstanceHelper
    {
        private const int HWND_BROADCAST = 0xffff;

        public static readonly int WM_SHOWAPP =
            RegisterWindowMessage($"WM_SHOWAPP|{AssemblyInfo.Guid}");

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SendMessageTimeout(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam, uint fuFlags,
            uint uTimeout, out IntPtr lpdwResult);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool SendNotifyMessage(IntPtr hWnd, uint Msg, UIntPtr wParam, IntPtr lParam);

        [DllImport("user32")]
        private static extern int RegisterWindowMessage(string message);


        public static void ShowFirstInstance()
        {
            SendNotifyMessage(
                (IntPtr) HWND_BROADCAST,
                (uint) WM_SHOWAPP,
                UIntPtr.Zero,
                IntPtr.Zero);
        }
    }
}