#region

using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#endregion

namespace DesktopWidgets.Classes
{
    internal static class IconUtilities
    {
        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool DeleteObject(IntPtr hObject);

        [DllImport("Shell32.dll", EntryPoint = "ExtractIconExW", CharSet = CharSet.Unicode, ExactSpelling = true,
            CallingConvention = CallingConvention.StdCall)]
        private static extern int ExtractIconEx(string sFile, int iIndex, out IntPtr piLargeVersion,
            out IntPtr piSmallVersion, int amountIcons);

        public static ImageSource ToImageSource(this Icon icon)
        {
            var bitmap = icon.ToBitmap();
            var hBitmap = bitmap.GetHbitmap();

            ImageSource wpfBitmap = Imaging.CreateBitmapSourceFromHBitmap(
                hBitmap,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            if (!DeleteObject(hBitmap))
            {
                throw new Win32Exception();
            }

            return wpfBitmap;
        }

        public static Icon Extract(string file, int number, bool largeIcon)
        {
            IntPtr large;
            IntPtr small;
            ExtractIconEx(file, number, out large, out small, 1);
            try
            {
                return Icon.FromHandle(largeIcon ? large : small);
            }
            catch
            {
                return null;
            }
        }
    }
}