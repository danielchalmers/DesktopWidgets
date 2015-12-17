#region

using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;

#endregion

namespace DesktopWidgets.Classes
{
    public class Win32App
    {
        private const int WS_EX_TRANSPARENT = 0x00000020;
        private const int GWL_EXSTYLE = (-20);

        public Win32App(IntPtr value)
        {
            hwnd = value;
        }

        private IntPtr hwnd { get; }

        public string GetTitle()
        {
            const int nChars = 256;
            var builder = new StringBuilder(nChars);
            return NativeMethods.GetWindowText(hwnd, builder, nChars) > 0 ? builder.ToString() : null;
        }

        public Win32Rect GetBounds()
        {
            Win32Rect appBounds;
            NativeMethods.GetWindowRect(hwnd, out appBounds);
            return appBounds;
        }

        public Screen GetScreen()
        {
            var bounds = GetBounds();
            return Screen.AllScreens
                .FirstOrDefault(scr => scr.Bounds.Contains(bounds.Left, bounds.Top));
        }

        public bool IsFullScreen()
        {
            return Screen.AllScreens.Any(IsFullScreen);
        }

        public bool IsFullScreen(Screen screen)
        {
            return screen != null && GetBounds().Equals(new Win32Rect
            {
                Left = screen.Bounds.Left,
                Top = screen.Bounds.Top,
                Right = screen.Bounds.Right,
                Bottom = screen.Bounds.Bottom
            }) && !(hwnd.Equals(NativeMethods.GetDesktopWindow()) || hwnd.Equals(NativeMethods.GetShellWindow()));
        }

        public void SetWindowExTransparent()
        {
            var extendedStyle = NativeMethods.GetWindowLong(hwnd, GWL_EXSTYLE);
            NativeMethods.SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);
        }
    }
}