using System;
using DesktopWidgets.Properties;

namespace DesktopWidgets.Helpers
{
    public static class DoubleHelper
    {
        public static bool IsEqual(this double val1, double val2) =>
            double.IsNaN(val1) || double.IsNaN(val1) ||
            (Math.Abs(val1 - val2) > Settings.Default.DoubleComparisonTolerance);
    }
}