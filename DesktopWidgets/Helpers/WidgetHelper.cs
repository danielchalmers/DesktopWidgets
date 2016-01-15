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
            Widgets.Sidebar.Metadata.FriendlyName,
            Widgets.Calculator.Metadata.FriendlyName,
            Widgets.FolderWatcher.Metadata.FriendlyName,
            Widgets.RSSFeed.Metadata.FriendlyName
        }.OrderBy(x => x).ToList();

        public static WidgetSettingsBase GetSettings(this WidgetId id)
        {
            return App.WidgetsSettingsStore.Widgets.FirstOrDefault(v => v.Identifier == id);
        }

        public static WidgetView GetView(this WidgetId id)
        {
            return App.WidgetViews.FirstOrDefault(v => v.Id == id);
        }

        public static string GetName(this WidgetId id)
        {
            var settings = id.GetSettings();
            var index = App.WidgetsSettingsStore.Widgets.IndexOf(settings);
            var name = settings.Name == "" ? $"Widget {index + 1}" : settings.Name;
            return $"{name} ({id.GetFriendlyName()})";
        }

        public static void NewWidget()
        {
            var dialog =
                new SelectItem(AvailableWidgets, "Widget");
            dialog.ShowDialog();
            if (dialog.SelectedItem == null)
                return;
            AddNewWidget((string) dialog.SelectedItem);
        }

        private static string GetFriendlyName(this WidgetId id)
        {
            var settings = id.GetSettings();
            if (settings is Settings)
            {
                return Metadata.FriendlyName;
            }
            if (settings is Widgets.CountdownClock.Settings)
            {
                return Widgets.CountdownClock.Metadata.FriendlyName;
            }
            if (settings is Widgets.StopwatchClock.Settings)
            {
                return Widgets.StopwatchClock.Metadata.FriendlyName;
            }
            if (settings is Widgets.Weather.Settings)
            {
                return Widgets.Weather.Metadata.FriendlyName;
            }
            if (settings is Widgets.Search.Settings)
            {
                return Widgets.Search.Metadata.FriendlyName;
            }
            if (settings is Widgets.Note.Settings)
            {
                return Widgets.Note.Metadata.FriendlyName;
            }
            if (settings is Widgets.PictureFrame.Settings)
            {
                return Widgets.PictureFrame.Metadata.FriendlyName;
            }
            if (settings is Widgets.PictureSlideshow.Settings)
            {
                return Widgets.PictureSlideshow.Metadata.FriendlyName;
            }
            if (settings is Widgets.Sidebar.Settings)
            {
                return Widgets.Sidebar.Metadata.FriendlyName;
            }
            if (settings is Widgets.Calculator.Settings)
            {
                return Widgets.Calculator.Metadata.FriendlyName;
            }
            if (settings is Widgets.FolderWatcher.Settings)
            {
                return Widgets.FolderWatcher.Metadata.FriendlyName;
            }
            if (settings is Widgets.RSSFeed.Settings)
            {
                return Widgets.RSSFeed.Metadata.FriendlyName;
            }
            return string.Empty;
        }

        private static WidgetSettingsBase GetNewSettingsFromFriendlyName(string name)
        {
            switch (name)
            {
                case Metadata.FriendlyName:
                    return new Settings();
                case Widgets.CountdownClock.Metadata.FriendlyName:
                    return new Widgets.CountdownClock.Settings();
                case Widgets.StopwatchClock.Metadata.FriendlyName:
                    return new Widgets.StopwatchClock.Settings();
                case Widgets.Weather.Metadata.FriendlyName:
                    return new Widgets.Weather.Settings();
                case Widgets.Search.Metadata.FriendlyName:
                    return new Widgets.Search.Settings();
                case Widgets.Note.Metadata.FriendlyName:
                    return new Widgets.Note.Settings();
                case Widgets.PictureFrame.Metadata.FriendlyName:
                    return new Widgets.PictureFrame.Settings();
                case Widgets.PictureSlideshow.Metadata.FriendlyName:
                    return new Widgets.PictureSlideshow.Settings();
                case Widgets.Sidebar.Metadata.FriendlyName:
                    return new Widgets.Sidebar.Settings();
                case Widgets.Calculator.Metadata.FriendlyName:
                    return new Widgets.Calculator.Settings();
                case Widgets.FolderWatcher.Metadata.FriendlyName:
                    return new Widgets.FolderWatcher.Settings();
                case Widgets.RSSFeed.Metadata.FriendlyName:
                    return new Widgets.RSSFeed.Settings();
                default:
                    return null;
            }
        }

        public static void LoadView(this WidgetId id)
        {
            var settings = id.GetSettings();
            UserControl userControl;
            WidgetViewModelBase dataContext;

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
            else if (settings is Widgets.Calculator.Settings)
            {
                dataContext = new Widgets.Calculator.ViewModel(id);
                userControl = new Widgets.Calculator.ControlView();
            }
            else if (settings is Widgets.FolderWatcher.Settings)
            {
                dataContext = new Widgets.FolderWatcher.ViewModel(id);
                userControl = new Widgets.FolderWatcher.ControlView();
            }
            else if (settings is Widgets.RSSFeed.Settings)
            {
                dataContext = new Widgets.RSSFeed.ViewModel(id);
                userControl = new Widgets.RSSFeed.ControlView();
            }
            else
            {
                return;
            }

            var widgetView = new WidgetView(id, dataContext, userControl);
            App.WidgetViews.Add(widgetView);
            ((Window) widgetView).Show();
        }

        private static void AddNewWidget(string type)
        {
            var newWidget = GetNewSettingsFromFriendlyName(type);
            App.WidgetsSettingsStore.Widgets.Add(newWidget);
            newWidget.Identifier.LoadView();
        }

        private static void AddNewWidget(WidgetSettingsBase settings)
        {
            App.WidgetsSettingsStore.Widgets.Add(settings);
            settings.Identifier.LoadView();
        }

        private static void Disable(this WidgetId id)
        {
            var settings = id.GetSettings();
            if (settings.Disabled)
                return;
            settings.Disabled = true;
            var view = id.GetView();
            if (view == null)
                return;
            view.Animate(AnimationMode.Hide, null, view.Close);
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
            var settings = id.GetSettings();
            if (msg && Popup.Show($"Are you sure you want to delete \"{settings.Name}\"?\n\nThis cannot be undone.",
                MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes) == MessageBoxResult.No)
                return;

            App.WidgetsSettingsStore.Widgets.Remove(settings);
            var view = id.GetView();
            if (view != null)
            {
                view.Close();
                App.WidgetViews.Remove(view);
            }
        }

        public static void Edit(this WidgetId id)
        {
            var settings = id.GetSettings();
            var previousHorizontalAlignment = settings.HorizontalAlignment;
            var previousVerticalAlignment = settings.VerticalAlignment;
            var previousIsDocked = settings.IsDocked;
            new EditWidget(id).ShowDialog();
            id.GetView()?
                .UpdateUi(isDocked: previousIsDocked, dockHorizontalAlignment: previousHorizontalAlignment,
                    dockVerticalAlignment: previousVerticalAlignment);
        }

        public static void LoadWidgetViews()
        {
            if (App.WidgetViews != null)
                foreach (var view in App.WidgetViews)
                    view.Close();
            App.WidgetViews = new ObservableCollection<WidgetView>();

            foreach (
                var id in
                    App.WidgetsSettingsStore.Widgets.Where(x => !x.Disabled).Select(settings => settings.Identifier))
                id.LoadView();
        }

        public static void ShowAllWidgetIntros()
        {
            foreach (var view in App.WidgetViews)
                view.ShowIntro();
        }

        public static void RefreshWidgets()
        {
            foreach (var view in App.WidgetViews)
                view.UpdateUi();
        }

        public static WidgetId Clone(this WidgetId id)
        {
            var newWidget = SettingsHelper.CloneObject(id.GetSettings()) as WidgetSettingsBase;
            if (newWidget != null)
            {
                newWidget.Identifier.GenerateNewGuid();
                AddNewWidget(newWidget);
            }
            return newWidget?.Identifier;
        }

        public static void Import(object widgetData)
        {
            var newWidget = SettingsHelper.CloneObject(widgetData) as WidgetSettingsBase;
            if (newWidget == null)
                return;
            newWidget.Identifier.GenerateNewGuid();
            AddNewWidget(newWidget);
        }
    }
}