using System;

namespace DesktopWidgets.Classes
{
    public class WidgetId
    {
        public WidgetId()
        {
            Guid = Guid.NewGuid();
        }

        public WidgetId(Guid guid)
        {
            Guid = guid;
        }

        public Guid Guid { get; private set; }

        public void GenerateNewGuid()
        {
            Guid = Guid.NewGuid();
        }
    }
}