using DesktopWidgets.WidgetBase;

namespace DesktopWidgets.Actions
{
    public class WidgetActionBase : IAction
    {
        public WidgetId WidgetId { get; set; }

        public virtual void Execute()
        {
        }
    }
}