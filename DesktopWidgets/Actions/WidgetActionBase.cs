using System.ComponentModel;
using DesktopWidgets.WidgetBase;

namespace DesktopWidgets.Actions
{
    public class WidgetActionBase : IAction
    {
        [DisplayName("Widget")]
        public WidgetId WidgetId { get; set; }

        public virtual void Execute()
        {
        }
    }
}