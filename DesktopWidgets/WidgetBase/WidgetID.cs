using System;

namespace DesktopWidgets.WidgetBase
{
    public class WidgetId
    {
        public Guid Guid { get; set; } = Guid.NewGuid();

        public void GenerateNewGuid()
        {
            Guid = Guid.NewGuid();
        }
    }
}