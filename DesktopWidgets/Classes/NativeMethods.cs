﻿using System;
using System.Runtime.InteropServices;
using System.Text;
using DesktopWidgets.Helpers;

namespace DesktopWidgets.Classes
{
    internal static class NativeMethods
    {
        internal const uint SWP_NOSIZE = 0x0001;
        internal const uint SWP_NOMOVE = 0x0002;
        internal const uint SWP_SHOWWINDOW = 0x0040;

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern int GetWindowRect(IntPtr hWnd, out Win32Rect rc);

        [DllImport("user32.dll")]
        internal static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll")]
        internal static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        internal static extern IntPtr GetShellWindow();

        [DllImport("user32.dll")]
        internal static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        internal static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        [DllImport("gdi32.dll", SetLastError = true)]
        internal static extern bool DeleteObject(IntPtr hObject);

        [DllImport("Shell32.dll", EntryPoint = "ExtractIconExW", CharSet = CharSet.Unicode, ExactSpelling = true,
            CallingConvention = CallingConvention.StdCall)]
        internal static extern int ExtractIconEx(string sFile, int iIndex, out IntPtr piLargeVersion,
            out IntPtr piSmallVersion, int amountIcons);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool SendNotifyMessage(IntPtr hWnd, uint Msg, UIntPtr wParam, IntPtr lParam);

        [DllImport("user32")]
        internal static extern int RegisterWindowMessage(string message);

        [DllImport("shell32.dll")]
        internal static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes,
            ref IconHelper.SHFILEINFO psfi,
            uint cbSizeFileInfo, uint uFlags);

        [DllImport("user32.dll")]
        internal static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        internal static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        internal static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy,
            uint uFlags);
    }
}