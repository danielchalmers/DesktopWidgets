using System.Windows.Input;
using DesktopWidgets.Classes;
using DesktopWidgets.Helpers;
using DesktopWidgets.Properties;
using DesktopWidgets.WidgetBase.Settings;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace DesktopWidgets.WindowViewModels
{
    public class ManageWidgetsViewModel : ViewModelBase
    {
        private WidgetSettingsBase _selectedWidget;

        public ManageWidgetsViewModel()
        {
            MouseDoubleClick = new RelayCommand<MouseButtonEventArgs>(MouseDoubleClickExecute);
            DeselectAll = new RelayCommand(DeselectAllExecute);
            NewWidget = new RelayCommand(NewWidgetExecute);
            EditWidget = new RelayCommand(EditWidgetExecute);
            MoveUpWidget = new RelayCommand(MoveUpWidgetExecute);
            MoveDownWidget = new RelayCommand(MoveDownWidgetExecute);
            ReloadWidget = new RelayCommand(ReloadWidgetExecute);
            MuteUnmuteWidget = new RelayCommand(MuteUnmuteWidgetExecute);
            DisableWidget = new RelayCommand(DisableWidgetExecute);
            RemoveWidget = new RelayCommand(RemoveWidgetExecute);
            CloneWidget = new RelayCommand(CloneWidgetExecute);
            ExportWidget = new RelayCommand(ExportWidgetExecute);
            ImportWidget = new RelayCommand(ImportWidgetExecute);
        }

        public WidgetSettingsBase SelectedWidget
        {
            get { return _selectedWidget; }
            set
            {
                if (_selectedWidget != value)
                {
                    _selectedWidget = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ICommand MouseDoubleClick { get; private set; }

        public ICommand DeselectAll { get; private set; }

        public ICommand NewWidget { get; private set; }

        public ICommand EditWidget { get; private set; }

        public ICommand MoveUpWidget { get; private set; }

        public ICommand MoveDownWidget { get; private set; }

        public ICommand ReloadWidget { get; private set; }

        public ICommand MuteUnmuteWidget { get; private set; }

        public ICommand DisableWidget { get; private set; }

        public ICommand RemoveWidget { get; private set; }

        public ICommand CloneWidget { get; private set; }

        public ICommand ExportWidget { get; private set; }

        public ICommand ImportWidget { get; private set; }

        private void MouseDoubleClickExecute(MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                SelectedWidget?.Identifier?.GetView()?.ShowIntro(new IntroData {ExecuteFinishAction = true});
            }
        }

        private void DeselectAllExecute()
        {
            SelectedWidget = null;
        }

        private void NewWidgetExecute()
        {
            SelectedWidget = WidgetHelper.NewWidget();
        }

        private void EditWidgetExecute()
        {
            SelectedWidget.Identifier.Edit();
            RaisePropertyChanged(nameof(SelectedWidget));
        }

        private void MoveUpWidgetExecute()
        {
            SelectedWidget = SelectedWidget.Identifier.MoveUp();
        }

        private void MoveDownWidgetExecute()
        {
            SelectedWidget = SelectedWidget.Identifier.MoveDown();
        }

        private void ReloadWidgetExecute()
        {
            SelectedWidget.Identifier.Reload();
        }

        private void MuteUnmuteWidgetExecute()
        {
            SelectedWidget.Identifier.ToggleMute(Settings.Default.MuteDuration);
            RaisePropertyChanged(nameof(SelectedWidget));
        }

        private void DisableWidgetExecute()
        {
            SelectedWidget.Identifier.ToggleEnable();
            RaisePropertyChanged(nameof(SelectedWidget));
        }

        private void RemoveWidgetExecute()
        {
            SelectedWidget.Identifier.Remove(true);
            DeselectAllExecute();
        }

        private void CloneWidgetExecute()
        {
            SelectedWidget = SelectedWidget.Identifier.Clone();
        }

        private void ExportWidgetExecute()
        {
            WidgetHelper.Export(SelectedWidget);
        }

        private void ImportWidgetExecute()
        {
            WidgetHelper.Import();
        }
    }
}