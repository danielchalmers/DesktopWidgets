using System.ComponentModel;
using DesktopWidgets.WidgetBase;

namespace DesktopWidgets.Actions
{
    public abstract class WidgetActionBase : ActionBase
    {
        [DisplayName("Widget")]
        public WidgetId WidgetId { get; set; }

        protected override void ExecuteAction()
        {
            base.ExecuteAction();
        }
    }
}