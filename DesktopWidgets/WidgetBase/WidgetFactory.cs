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
            Widgets.LatencyMonitor.Metadata.FriendlyName
        }.OrderBy(x => x).ToList();

        public static string GetFriendlyName(this WidgetId id)
        {
            var settings = id.GetSettings();
            if (settings is Widgets.TimeClock.Settings)
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
            if (settings is Widgets.LatencyMonitor.Settings)
            {
                return Widgets.LatencyMonitor.Metadata.FriendlyName;
            }
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
                case Widgets.LatencyMonitor.Metadata.FriendlyName:
                    return new Widgets.LatencyMonitor.Settings();
                default:
                    return null;
            }
        }

        public static WidgetViewModelBase GetNewViewModel(this WidgetId id)
        {
            var settings = id.GetSettings();
            if (settings is Widgets.TimeClock.Settings)
            {
                return new Widgets.TimeClock.ViewModel(id);
            }
            if (settings is Widgets.CountdownClock.Settings)
            {
                return new Widgets.CountdownClock.ViewModel(id);
            }
            if (settings is Widgets.StopwatchClock.Settings)
            {
                return new Widgets.StopwatchClock.ViewModel(id);
            }
            if (settings is Widgets.Weather.Settings)
            {
                return new Widgets.Weather.ViewModel(id);
            }
            if (settings is Widgets.Search.Settings)
            {
                return new Widgets.Search.ViewModel(id);
            }
            if (settings is Widgets.Note.Settings)
            {
                return new Widgets.Note.ViewModel(id);
            }
            if (settings is Widgets.PictureSlideshow.Settings)
            {
                return new Widgets.PictureSlideshow.ViewModel(id);
            }
            if (settings is Widgets.Sidebar.Settings)
            {
                return new Widgets.Sidebar.ViewModel(id);
            }
            if (settings is Widgets.Calculator.Settings)
            {
                return new Widgets.Calculator.ViewModel(id);
            }
            if (settings is Widgets.FolderWatcher.Settings)
            {
                return new Widgets.FolderWatcher.ViewModel(id);
            }
            if (settings is Widgets.RSSFeed.Settings)
            {
                return new Widgets.RSSFeed.ViewModel(id);
            }
            if (settings is Widgets.LatencyMonitor.Settings)
            {
                return new Widgets.LatencyMonitor.ViewModel(id);
            }
            return null;
        }

        public static UserControl GetNewControlView(this WidgetId id)
        {
            var settings = id.GetSettings();
            if (settings is Widgets.TimeClock.Settings)
            {
                return new ControlView();
            }
            if (settings is Widgets.CountdownClock.Settings)
            {
                return new Widgets.CountdownClock.ControlView();
            }
            if (settings is Widgets.StopwatchClock.Settings)
            {
                return new Widgets.StopwatchClock.ControlView();
            }
            if (settings is Widgets.Weather.Settings)
            {
                return new Widgets.Weather.ControlView();
            }
            if (settings is Widgets.Search.Settings)
            {
                return new Widgets.Search.ControlView();
            }
            if (settings is Widgets.Note.Settings)
            {
                return new Widgets.Note.ControlView();
            }
            if (settings is Widgets.PictureSlideshow.Settings)
            {
                return new Widgets.PictureSlideshow.ControlView();
            }
            if (settings is Widgets.Sidebar.Settings)
            {
                return new Widgets.Sidebar.ControlView();
            }
            if (settings is Widgets.Calculator.Settings)
            {
                return new Widgets.Calculator.ControlView();
            }
            if (settings is Widgets.FolderWatcher.Settings)
            {
                return new Widgets.FolderWatcher.ControlView();
            }
            if (settings is Widgets.RSSFeed.Settings)
            {
                return new Widgets.RSSFeed.ControlView();
            }
            if (settings is Widgets.LatencyMonitor.Settings)
            {
                return new Widgets.LatencyMonitor.ControlView();
            }
            return null;
        }
    }
}