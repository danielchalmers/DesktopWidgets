using System.Collections.Generic;
using DesktopWidgets.Windows;

namespace DesktopWidgets.Classes
{
    public static class EditWidgetStore
    {
        private static readonly Dictionary<WidgetId, EditWidget> Editors = new Dictionary<WidgetId, EditWidget>();

        private static EditWidget GetEditWidgetWindow(WidgetId id)
        {
            if (!Editors.ContainsKey(id))
                Editors.Add(id, new EditWidget(id));
            return Editors[id];
        }

        public static void ShowEditWidgetWindow(WidgetId id)
        {
            var window = GetEditWidgetWindow(id);
            window.Show();
            window.Activate();
        }
    }
}