using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DesktopWidgets.Classes;
using DesktopWidgets.View;
using DesktopWidgets.ViewModelBase;
using DesktopWidgets.Widgets.TimeClock;
using DesktopWidgets.Windows;

namespace DesktopWidgets.Helpers
{
    public static class WidgetHelper
    {
        private static readonly List<string> AvailableWidgets = new List<string>
        {
            Metadata.FriendlyName,
            Widgets.CountdownClock.Metadata.FriendlyName,
            Widgets.StopwatchClock.Metadata.FriendlyName,
            Widgets.Weather.Metadata.FriendlyName,
            Widgets.Search.Metadata.FriendlyName,
            Widgets.Note.Metadata.FriendlyName,
            Widgets.PictureFrame.Metadata.FriendlyName,
            Widgets.PictureSlideshow.Metadata.FriendlyName,
            Widgets.Sidebar.Metadata.FriendlyName
        }.OrderBy(x => x).ToList();

        public static WidgetSettings GetSettings(this WidgetId id)
        {
            return App.WidgetsSettingsStore.Widgets.First(v => v.ID == id);
        }

        public static WidgetView GetView(this WidgetId id)
        {
            return App.WidgetViews.First(v => v.Id == id);
        }

        //public static WidgetViewModelBase GetViewModel(this WidgetId id)
        //{
        //    return App.WidgetViews.First(v => v.Id == id).DataContext as WidgetViewModelBase;
        //}

        public static string GetName(this WidgetId id)
        {
            var settings = id.GetSettings();
            var index = App.WidgetsSettingsStore.Widgets.IndexOf(settings);
            var name = (settings.Name == "" ? $"Widget {index + 1}" : settings.Name);
            return $"{name}";
        }

        public static void NewWidget()
        {
            var dialog =
                new SelectItem(AvailableWidgets);
            dialog.ShowDialog();
            AddNewWidget((string) dialog.SelectedItem);
        }

        private static void AddNewWidget(string type)
        {
            WidgetSettings newWidget;
            switch (type)
            {
                case Metadata.FriendlyName:
                    newWidget = new Settings();
                    break;
                case Widgets.CountdownClock.Metadata.FriendlyName:
                    newWidget = new Widgets.CountdownClock.Settings();
                    break;
                case Widgets.StopwatchClock.Metadata.FriendlyName:
                    newWidget = new Widgets.StopwatchClock.Settings();
                    break;
                case Widgets.Weather.Metadata.FriendlyName:
                    newWidget = new Widgets.Weather.Settings();
                    break;
                case Widgets.Search.Metadata.FriendlyName:
                    newWidget = new Widgets.Search.Settings();
                    break;
                case Widgets.Note.Metadata.FriendlyName:
                    newWidget = new Widgets.Note.Settings();
                    break;
                case Widgets.PictureFrame.Metadata.FriendlyName:
                    newWidget = new Widgets.PictureFrame.Settings();
                    break;
                case Widgets.PictureSlideshow.Metadata.FriendlyName:
                    newWidget = new Widgets.PictureSlideshow.Settings();
                    break;
                case Widgets.Sidebar.Metadata.FriendlyName:
                    newWidget = new Widgets.Sidebar.Settings();
                    break;
                default:
                    return;
            }
            App.WidgetsSettingsStore.Widgets.Add(newWidget);
            newWidget.ID.LoadView();
        }

        private static void Disable(this WidgetId id)
        {
            var settings = id.GetSettings();
            if (settings.Disabled)
                return;
            settings.Disabled = true;
            var view = id.GetView();
            view.Close();
            App.WidgetViews.Remove(view);
        }

        private static void Enable(this WidgetId id)
        {
            var settings = id.GetSettings();
            if (!settings.Disabled)
                return;
            settings.Disabled = false;
            id.LoadView();
        }

        public static void ToggleEnable(this WidgetId id)
        {
            if (id.GetSettings().Disabled)
                id.Enable();
            else
                id.Disable();
        }

        public static void Remove(this WidgetId id, bool msg = false)
        {
            if (msg && Popup.Show("Are you sure you want to delete this widget?\n\nThis cannot be undone.",
                MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes) == MessageBoxResult.No)
                return;

            var view = id.GetView();
            var settings = id.GetSettings();
            view.Close();
            App.WidgetsSettingsStore.Widgets.Remove(settings);
            App.WidgetViews.Remove(view);
        }

        public static void Edit(this WidgetId id)
        {
            new PropertyView(id.GetSettings()).ShowDialog();
            id.GetView().UpdateUi();
        }

        public static void LoadView(this WidgetId id)
        {
            var settings = id.GetSettings();
            var widgetView = new WidgetView(id);
            var userControlStyle = (Style) widgetView.FindResource("UserControlStyle");
            UserControl userControl;
            WidgetViewModelBase dataContext;

            App.WidgetViews.Add(widgetView);

            if (settings is Settings)
            {
                dataContext = new Widgets.TimeClock.ViewModel(id);
                userControl = new ControlView();
            }
            else if (settings is Widgets.CountdownClock.Settings)
            {
                dataContext = new Widgets.CountdownClock.ViewModel(id);
                userControl = new Widgets.CountdownClock.ControlView();
            }
            else if (settings is Widgets.StopwatchClock.Settings)
            {
                dataContext = new Widgets.StopwatchClock.ViewModel(id);
                userControl = new Widgets.StopwatchClock.ControlView();
            }
            else if (settings is Widgets.Weather.Settings)
            {
                dataContext = new Widgets.Weather.ViewModel(id);
                userControl = new Widgets.Weather.ControlView();
            }
            else if (settings is Widgets.Search.Settings)
            {
                dataContext = new Widgets.Search.ViewModel(id);
                userControl = new Widgets.Search.ControlView();
            }
            else if (settings is Widgets.Note.Settings)
            {
                dataContext = new Widgets.Note.ViewModel(id);
                userControl = new Widgets.Note.ControlView();
            }
            else if (settings is Widgets.PictureFrame.Settings)
            {
                dataContext = new Widgets.PictureFrame.ViewModel(id);
                userControl = new Widgets.PictureFrame.ControlView();
            }
            else if (settings is Widgets.PictureSlideshow.Settings)
            {
                dataContext = new Widgets.PictureSlideshow.ViewModel(id);
                userControl = new Widgets.PictureSlideshow.ControlView();
            }
            else if (settings is Widgets.Sidebar.Settings)
            {
                dataContext = new Widgets.Sidebar.ViewModel(id);
                userControl = new Widgets.Sidebar.ControlView();
            }
            else
            {
                return;
            }

            userControl.Style = userControlStyle;
            widgetView.DataContext = dataContext;
            widgetView.MainContentContainer.Child = userControl;

            widgetView.Show();
        }

        public static void LoadWidgets()
        {
            if (App.WidgetViews != null)
                foreach (var view in App.WidgetViews)
                    view.Close();
            App.WidgetViews = new ObservableCollection<WidgetView>();

            if (App.WidgetsSettingsStore == null)
                App.WidgetsSettingsStore = new WidgetsSettingsStore
                {
                    Widgets = new ObservableCollection<WidgetSettings>()
                };
            App.WidgetsSettingsStore.Widgets.CollectionChanged += (sender, args) => App.SaveTimer.DelaySave();

            foreach (
                var id in App.WidgetsSettingsStore.Widgets.Where(x => !x.Disabled).Select(settings => settings.ID))
                id.LoadView();
        }
    }
}