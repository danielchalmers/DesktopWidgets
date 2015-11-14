using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DesktopWidgets
{
    public static class WindowHelper
    {
        public static void SnapToScreenEdges(this Window window)
        {
            const int snappingMargin = 5;

            if (Math.Abs(SystemParameters.WorkArea.Left - window.Left) < snappingMargin)
                window.Left = SystemParameters.WorkArea.Left;
            else if (
                Math.Abs(window.Left + window.ActualWidth - SystemParameters.WorkArea.Left -
                         SystemParameters.WorkArea.Width) <
                snappingMargin)
                window.Left = SystemParameters.WorkArea.Left + SystemParameters.WorkArea.Width - window.ActualWidth;

            if (Math.Abs(SystemParameters.WorkArea.Top - window.Top) < snappingMargin)
                window.Top = SystemParameters.WorkArea.Top;
            else if (
                Math.Abs(window.Top + window.ActualHeight - SystemParameters.WorkArea.Top -
                         SystemParameters.WorkArea.Height) <
                snappingMargin)
                window.Top = SystemParameters.WorkArea.Top + SystemParameters.WorkArea.Height - window.ActualHeight;
        }
    }
}
