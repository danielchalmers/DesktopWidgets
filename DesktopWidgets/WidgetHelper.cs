using System;
using System.Linq;

namespace DesktopWidgets
{
    public static class WidgetHelper
    {
        public static WidgetSettings GetWidgetSettingsFromGuid(Guid guid)
        {
            return App.WidgetCfg.Widgets.First(v => v.Guid == guid);
        }
    }
}