using System.ComponentModel;
using DesktopWidgets.Helpers;

namespace DesktopWidgets.Actions
{
    public class WidgetChangeSizeAction : WidgetActionBase
    {
        [DisplayName("Width")]
        public double Width { get; set; } = double.NaN;

        [DisplayName("Height")]
        public double Height { get; set; } = double.NaN;

        [DisplayName("Size Change Mode")]
        public SizeChangeMode SizeChangeMode { get; set; } = SizeChangeMode.Both;

        [DisplayName("Number Change Mode")]
        public NumberChangeMode NumberChangeMode { get; set; } = NumberChangeMode.Set;

        protected override void ExecuteAction()
        {
            base.ExecuteAction();
            var settings = WidgetId?.GetSettings();
            if (settings == null)
                return;
            if (SizeChangeMode == SizeChangeMode.Both || SizeChangeMode == SizeChangeMode.Width)
            {
                switch (NumberChangeMode)
                {
                    case NumberChangeMode.Increase:
                        settings.Style.Width += Width;
                        break;
                    case NumberChangeMode.Decrease:
                        settings.Style.Width -= Width;
                        break;
                    case NumberChangeMode.Set:
                        settings.Style.Width = Width;
                        break;
                }
            }
            if (SizeChangeMode == SizeChangeMode.Both || SizeChangeMode == SizeChangeMode.Height)
            {
                switch (NumberChangeMode)
                {
                    case NumberChangeMode.Increase:
                        settings.Style.Height += Height;
                        break;
                    case NumberChangeMode.Decrease:
                        settings.Style.Height -= Height;
                        break;
                    case NumberChangeMode.Set:
                        settings.Style.Height = Height;
                        break;
                }
            }
        }
    }
}