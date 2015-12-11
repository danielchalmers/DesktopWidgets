using System.Collections;
using System.ComponentModel;
using System.Windows;

namespace DesktopWidgets.OptionsPages
{
    public partial class WidgetOptionsPagesBase : ResourceDictionary, INotifyPropertyChanged
    {
        private IEnumerable _openModeItems;

        public WidgetOptionsPagesBase()
        {
            InitializeComponent();
            //OpenModeItems = Enum.GetValues(typeof(OpenMode));
            //cbAnimationType.ItemsSource = Enum.GetValues(typeof(AnimationType));
            //cbDockAlignment.ItemsSource = Enum.GetValues(typeof(ScreenDockAlignment));
            //cbDockPosition.ItemsSource = Enum.GetValues(typeof(ScreenDockPosition));
            //cbNameAlignment.ItemsSource = Enum.GetValues(typeof(TextAlignment));
        }

        public IEnumerable OpenModeItems
        {
            get { return _openModeItems; }
            set
            {
                if (_openModeItems != value)
                {
                    _openModeItems = value;
                    RaisePropertyChanged(nameof(OpenModeItems));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}