using System.ComponentModel;
using System.Windows;

namespace DesktopWidgets.Actions
{
    internal class PopupAction : ActionBase
    {
        [DisplayName("Text")]
        public string Text { get; set; } = "";

        [DisplayName("Title")]
        public string Title { get; set; } = "";

        [DisplayName("Image")]
        public MessageBoxImage Image { get; set; }

        protected override void ExecuteAction()
        {
            base.ExecuteAction();
            MessageBox.Show(Text, Title, MessageBoxButton.OK, Image);
        }
    }
}