using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using DesktopWidgets.Helpers;
using DesktopWidgets.WidgetBase.Settings;
using DesktopWidgets.WidgetBase.ViewModel;
using DesktopWidgets.Widgets.TimeClock;

namespace DesktopWidgets.WidgetBase
{
    public static class WidgetFactory
    {
        public static readonly List<string> AvailableWidgets = new List<string>
        {
            Metadata.FriendlyName,
            Widgets.CountdownClock.Metadata.FriendlyName,
            Widgets.StopwatchClock.Metadata.FriendlyName,
            Widgets.Weather.Metadata.FriendlyName,
            Widgets.Search.Metadata.FriendlyName,
            Widgets.Note.Metadata.FriendlyName,
            Widgets.PictureSlideshow.Metadata.FriendlyName,
            Widgets.Sidebar.Metadata.FriendlyName,
            Widgets.Calculator.Metadata.FriendlyName,
            Widgets.FolderWatcher.Metadata.FriendlyName,
            Widgets.RSSFeed.Metadata.FriendlyName,
            Widgets.CommandButton.Metadata.FriendlyName
        }.OrderBy(x => x).ToList();

        public static string GetFriendlyName(this WidgetId id)
        {
            var settings = id.GetSettings();
            if (settings is Widgets.TimeClock.Settings)
                return Metadata.FriendlyName;
            if (settings is Widgets.CountdownClock.Settings)
                return Widgets.CountdownClock.Metadata.FriendlyName;
            if (settings is Widgets.StopwatchClock.Settings)
                return Widgets.StopwatchClock.Metadata.FriendlyName;
            if (settings is Widgets.Weather.Settings)
                return Widgets.Weather.Metadata.FriendlyName;
            if (settings is Widgets.Search.Settings)
                return Widgets.Search.Metadata.FriendlyName;
            if (settings is Widgets.Note.Settings)
                return Widgets.Note.Metadata.FriendlyName;
            if (settings is Widgets.PictureSlideshow.Settings)
                return Widgets.PictureSlideshow.Metadata.FriendlyName;
            if (settings is Widgets.Sidebar.Settings)
                return Widgets.Sidebar.Metadata.FriendlyName;
            if (settings is Widgets.Calculator.Settings)
                return Widgets.Calculator.Metadata.FriendlyName;
            if (settings is Widgets.FolderWatcher.Settings)
                return Widgets.FolderWatcher.Metadata.FriendlyName;
            if (settings is Widgets.RSSFeed.Settings)
                return Widgets.RSSFeed.Metadata.FriendlyName;
            if (settings is Widgets.CommandButton.Settings)
                return Widgets.CommandButton.Metadata.FriendlyName;
            return null;
        }

        public static WidgetSettingsBase GetNewSettingsFromFriendlyName(string name)
        {
            switch (name)
            {
                case Metadata.FriendlyName:
                    return new Widgets.TimeClock.Settings();
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
                case Widgets.CommandButton.Metadata.FriendlyName:
                    return new Widgets.CommandButton.Settings();
                default:
                    return null;
            }
        }

        public static Tuple<WidgetViewModelBase, UserControl> GetViewData(WidgetId id)
        {
            var settings = id.GetSettings();
            UserControl userControl = null;
            WidgetViewModelBase dataContext = null;

            if (settings is Widgets.TimeClock.Settings)
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
            else if (settings is Widgets.CommandButton.Settings)
            {
                dataContext = new Widgets.CommandButton.ViewModel(id);
                userControl = new Widgets.CommandButton.ControlView();
            }
            return new Tuple<WidgetViewModelBase, UserControl>(dataContext, userControl);
        }
    }
}