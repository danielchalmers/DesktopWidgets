#region

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#endregion

namespace DesktopWidgets.Helpers
{
    internal class IconHelper
    {
        private const uint SHGFI_ICON = 0x100;
        private const uint SHGFI_LARGEICON = 0x0;

        [DllImport("shell32.dll")]
        private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi,
            uint cbSizeFileInfo, uint uFlags);

        public static ImageSource GetPathIcon(string path)
        {
            var shinfo = new SHFILEINFO();
            SHGetFileInfo(
                path,
                0, ref shinfo, (uint) Marshal.SizeOf(shinfo),
                SHGFI_ICON | SHGFI_LARGEICON);
            using (var i = Icon.FromHandle(shinfo.hIcon))
                return Imaging.CreateBitmapSourceFromHIcon(
                    i.Handle,
                    new Int32Rect(0, 0, i.Width, i.Height),
                    BitmapSizeOptions.FromEmptyOptions());
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SHFILEINFO
        {
            public readonly IntPtr hIcon;
            private readonly int iIcon;
            private readonly uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)] private readonly string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)] private readonly string szTypeName;
        };
    }
}