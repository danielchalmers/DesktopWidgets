#region

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using DesktopWidgets.Classes;
using DesktopWidgets.Helpers;
using DesktopWidgets.OptionsPages;
using Advanced = DesktopWidgets.WidgetBase.OptionsPages.Advanced;

#endregion

namespace DesktopWidgets.Windows
{
    /// <summary>
    ///     Interaction logic for Options.xaml
    /// </summary>
    public partial class Options : Window, INotifyPropertyChanged
    {
        private Page _currentPage;

        public Options(WidgetId id = null)
        {
            InitializeComponent();

            _id = id;
            Settings = id.GetSettings();

            Pages = new ObservableCollection<Page>();
            AddPages();
            foreach (var page in Pages)
                page.DataContext = this;
            if (Pages.Count > 0)
                CurrentPage = Pages[0];

            OpenModeItems = Enum.GetValues(typeof (OpenMode));
            AnimationTypeItems = Enum.GetValues(typeof (AnimationType));
            DockAlignmentItems = Enum.GetValues(typeof (ScreenDockAlignment));
            DockPositionItems = Enum.GetValues(typeof (ScreenDockPosition));
            NameAlignmentItems = Enum.GetValues(typeof (TextAlignment));

            IconPositionItems = Enum.GetValues(typeof (IconPosition));
            ToolTipTypeItems = Enum.GetValues(typeof (ToolTipType));
            ShortcutContentModeItems = Enum.GetValues(typeof (ShortcutContentMode));
            ButtonAlignmentItems = Enum.GetValues(typeof (ShortcutAlignment));
            ShortcutOrientationItems = Enum.GetValues(typeof (ShortcutOrientation));
            ScrollBarVisibilityItems = Enum.GetValues(typeof (ScrollBarVisibility));
            IconScalingModeItems = Enum.GetValues(typeof (ImageScalingMode));

            Properties.Settings.Default.Save();

            DataContext = this;
        }

        private WidgetId _id { get; }
        public WidgetSettingsBase Settings { get; set; }

        public IEnumerable OpenModeItems { get; }
        public IEnumerable AnimationTypeItems { get; }
        public IEnumerable DockAlignmentItems { get; }
        public IEnumerable DockPositionItems { get; }
        public IEnumerable NameAlignmentItems { get; }

        public IEnumerable IconPositionItems { get; }
        public IEnumerable ToolTipTypeItems { get; }
        public IEnumerable ShortcutContentModeItems { get; }
        public IEnumerable ButtonAlignmentItems { get; }
        public IEnumerable ShortcutOrientationItems { get; }
        public IEnumerable ScrollBarVisibilityItems { get; }
        public IEnumerable IconScalingModeItems { get; }

        public ObservableCollection<Page> Pages { get; }

        public Page CurrentPage
        {
            get { return _currentPage; }
            set
            {
                if (!Equals(_currentPage, value))
                {
                    _currentPage = value;
                    RaisePropertyChanged(nameof(CurrentPage));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private void AddPages()
        {
            Pages.Add(new General());

            if (_id != null)
            {
                foreach (var page in _id.GetOptionsPages())
                {
                    var stackPanel = page.Content as StackPanel;
                    switch (page.Title)
                    {
                        case "General":
                            stackPanel.Children.Add(
                                (UIElement) Application.Current.Resources["WidgetOptionsGeneralBase"]);
                            break;
                        case "Style":
                            stackPanel.Children.Add((UIElement) Application.Current.Resources["WidgetOptionsStyleBase"]);
                            break;
                    }
                    page.Content = stackPanel;
                    Pages.Add(page);
                }
                if (Properties.Settings.Default.EnableAdvancedMode)
                    Pages.Add(new Advanced(_id.GetSettings()));
            }

            if (Properties.Settings.Default.EnableAdvancedMode)
                Pages.Add(new OptionsPages.Advanced());
            Pages.Add(new About());
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            foreach (var be in Pages.SelectMany(BindingOperations.GetSourceUpdatingBindings))
                be.UpdateSource();
            Properties.Settings.Default.Save();
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Reload();
            Close();
        }

        private void frame_LoadCompleted(object sender, NavigationEventArgs e)
        {
            var content = (sender as Frame).Content as FrameworkElement;
            if (content == null)
                return;
            content.Style = (Style) FindResource("OptionsStyle");
        }
    }
}