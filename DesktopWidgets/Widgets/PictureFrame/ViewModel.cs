using System.Linq;
using System.Windows;
using DesktopWidgets.Classes;
using DesktopWidgets.Helpers;
using DesktopWidgets.ViewModelBase;
using DataFormats = System.Windows.Forms.DataFormats;

namespace DesktopWidgets.Widgets.PictureFrame
{
    public class ViewModel : WidgetViewModelBase
    {
        public ViewModel(WidgetId id) : base(id)
        {
            Settings = id.GetSettings() as Settings;
            if (Settings == null)
                return;
            AllowDrop = Settings.AllowDropFiles;
        }

        public Settings Settings { get; }

        public override void DropExecute(DragEventArgs e)
        {
            if (AllowDrop && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                Settings.ImageUrl = ((string[]) e.Data.GetData(System.Windows.DataFormats.FileDrop)).FirstOrDefault();
                _id.GetView()?.UpdateUi(true, false);
            }
        }
    }
}