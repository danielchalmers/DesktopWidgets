using System;

namespace DesktopWidgets.WidgetBase
{
    public class WidgetId
    {
        public WidgetId()
        {
            Guid = Guid.NewGuid();
        }

        public Guid Guid { get; private set; }

        public void GenerateNewGuid()
        {
            Guid = Guid.NewGuid();
        }
    }
}