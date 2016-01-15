using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Threading;
using System.Xml;
using DesktopWidgets.Classes;
using DesktopWidgets.Helpers;
using DesktopWidgets.ViewModelBase;
using GalaSoft.MvvmLight.CommandWpf;

namespace DesktopWidgets.Widgets.RSSFeed
{
    public class ViewModel : WidgetViewModelBase
    {
        public readonly DispatcherTimer UpdateTimer;
        private ObservableCollection<FeedItem> _feedItems;

        private string _lastFeedUrl;

        private bool _showHelp;

        public ViewModel(WidgetId id) : base(id)
        {
            Settings = id.GetSettings() as Settings;
            if (Settings == null)
                return;
            NavigateHyperlink = new RelayCommand<RequestNavigateEventArgs>(NavigateHyperlinkExecute);
            FeedItems = new ObservableCollection<FeedItem>();
            UpdateTimer = new DispatcherTimer {Interval = Settings.RefreshInterval};
            UpdateTimer.Tick += (sender, args) => UpdateFeed();

            RefreshAction = delegate
            {
                if (_lastFeedUrl != Settings.RssFeedUrl)
                    UpdateFeed();
            };

            UpdateFeed();
            UpdateTimer.Start();
        }

        public Settings Settings { get; }

        public ICommand NavigateHyperlink { get; set; }

        public bool ShowHelp
        {
            get { return _showHelp; }
            set
            {
                if (_showHelp != value)
                {
                    _showHelp = value;
                    RaisePropertyChanged(nameof(ShowHelp));
                }
            }
        }

        public ObservableCollection<FeedItem> FeedItems
        {
            get { return _feedItems; }
            set
            {
                if (_feedItems != value)
                {
                    _feedItems = value;
                    RaisePropertyChanged(nameof(FeedItems));
                }
            }
        }

        private void NewHeadlineFound()
        {
            MediaPlayerStore.PlaySoundAsync(Settings.EventSoundPath, Settings.EventSoundVolume);
            if (Settings.OpenOnEvent)
                Settings.Identifier.GetView()
                    .ShowIntro(Settings.OpenOnEventStay ? 0 : (int) Settings.OpenOnEventDuration.TotalMilliseconds,
                        false);
        }

        private void DownloadFeed(Action<SyndicationFeed> finishAction)
        {
            try
            {
                using (var wc = new WebClient())
                {
                    wc.DownloadStringCompleted +=
                        (sender, args) =>
                        {
                            var sr = new StringReader(args.Result);
                            var reader = XmlReader.Create(sr);
                            var feed = SyndicationFeed.Load(reader);
                            reader.Close();
                            sr.Close();
                            finishAction(feed);
                        };
                    wc.DownloadStringAsync(new Uri(Settings.RssFeedUrl));
                }
            }
            catch
            {
                // ignored
            }
        }

        private void UpdateFeed()
        {
            _lastFeedUrl = Settings.RssFeedUrl;

            if (string.IsNullOrWhiteSpace(Settings.RssFeedUrl))
            {
                ShowHelp = true;
                return;
            }
            ShowHelp = false;

            DownloadFeed(feed =>
            {
                if (feed?.Items != null)
                {
                    var prevFeed = FeedItems.ToList();
                    FeedItems.Clear();
                    var newHeadlineFound = false;
                    foreach (
                        var newItem in
                            feed.Items.Where(
                                x =>
                                    (string.IsNullOrWhiteSpace(Settings.CategoryFilter) ||
                                     Settings.CategoryFilter.Split(',').Any(y => x.Categories.Any(z => z.Name == y))) &&
                                    (string.IsNullOrWhiteSpace(Settings.RssFeedTitleWhitelist) ||
                                     Settings.RssFeedTitleWhitelist.Split(',').Any(y => x.Title.Text.Contains(y))) &&
                                    (string.IsNullOrWhiteSpace(Settings.RssFeedTitleBlacklist) ||
                                     Settings.RssFeedTitleBlacklist.Split(',').All(y => !x.Title.Text.Contains(y))))
                                .Select(
                                    item =>
                                        new FeedItem(item.Title.Text, item.Links.FirstOrDefault()?.Uri?.AbsoluteUri,
                                            (item.PublishDate.DateTime + Settings.PublishDateTimeOffsetPositive -
                                             Settings.PublishDateTimeOffsetNegative).ToString(Settings.PublishDateFormat)))
                        )
                    {
                        FeedItems.Add(newItem);
                        if (!prevFeed.Any(x => x.Title == newItem.Title && x.Hyperlink == newItem.Hyperlink))
                            newHeadlineFound = true;
                        if (FeedItems.Count >= Settings.MaxHeadlines && Settings.MaxHeadlines > 0)
                            break;
                    }
                    if (prevFeed.Count > 0 && newHeadlineFound)
                        NewHeadlineFound();
                }
            });
        }

        private void NavigateHyperlinkExecute(RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.AbsoluteUri);
        }
    }
}