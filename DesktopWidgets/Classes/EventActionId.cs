using System;

namespace DesktopWidgets.Classes
{
    public class EventActionId
    {
        public Guid Guid { get; set; } = Guid.NewGuid();

        public void GenerateNewGuid()
        {
            Guid = Guid.NewGuid();
        }
    }
}