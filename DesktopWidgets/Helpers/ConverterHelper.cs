using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace DesktopWidgets.Helpers
{
    public static class ConverterHelper
    {
        public static bool IsValueValid(IList<object> value)
        {
            return value != null && value.All(x => x != DependencyProperty.UnsetValue && x != null);
        }

        public static bool IsValueValid(object value)
        {
            return value != null;
        }
    }
}