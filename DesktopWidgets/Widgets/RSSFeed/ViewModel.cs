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
using DesktopWidgets.Helpers;
using DesktopWidgets.Stores;
using DesktopWidgets.WidgetBase;
using DesktopWidgets.WidgetBase.ViewModel;
using GalaSoft.MvvmLight.CommandWpf;

namespace DesktopWidgets.Widgets.RSSFeed
{
    public class ViewModel : WidgetViewModelBase
    {
        private ObservableCollection<FeedItem> _feedItems;

        private string _helpText;

        private string _lastFeedUrl;

        public DispatcherTimer UpdateTimer;

        public ViewModel(WidgetId id) : base(id)
        {
            Settings = id.GetSettings() as Settings;
            if (Settings == null)
                return;
            NavigateHyperlink = new RelayCommand<RequestNavigateEventArgs>(NavigateHyperlinkExecute);
            FeedItems = new ObservableCollection<FeedItem>();
            UpdateTimer = new DispatcherTimer {Interval = Settings.RefreshInterval};
            UpdateTimer.Tick += (sender, args) => UpdateFeed();

            UpdateFeed();
            UpdateTimer.Start();
        }

        public Settings Settings { get; }

        public ICommand NavigateHyperlink { get; set; }

        public string HelpText
        {
            get { return _helpText; }
            set
            {
                if (_helpText != value)
                {
                    _helpText = value;
                    RaisePropertyChanged(nameof(HelpText));
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
            if (!App.IsMuted)
                MediaPlayerStore.PlaySoundAsync(Settings.EventSoundPath, Settings.EventSoundVolume);
            if (Settings.OpenOnEvent)
                View?.ShowIntro(
                    new IntroData
                    {
                        Duration = Settings.OpenOnEventStay ? 0 : (int) Settings.OpenOnEventDuration.TotalMilliseconds
                    });
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
                HelpText = "Error";
            }
        }

        private void UpdateFeed()
        {
            _lastFeedUrl = Settings.RssFeedUrl;

            if (string.IsNullOrWhiteSpace(Settings.RssFeedUrl))
            {
                HelpText = "Enter an RSS URL";
                return;
            }
            HelpText = "Loading...";

            DownloadFeed(feed =>
            {
                if (feed?.Items == null)
                {
                    HelpText = "No feed items found";
                }
                else
                {
                    var prevFeed = FeedItems.ToList();
                    FeedItems = new ObservableCollection<FeedItem>(feed.Items
                        .Select(
                            item =>
                                new FeedItem(item.Title.Text, item.Links.FirstOrDefault()?.Uri?.AbsoluteUri,
                                    item.PublishDate.LocalDateTime, item.Categories)));
                    if (prevFeed.Count > 0 &&
                        FeedItems.Any(y => !prevFeed.Any(x => x.Title == y.Title && x.Hyperlink == y.Hyperlink)))
                        NewHeadlineFound();
                    HelpText = "";
                }
            });
        }

        private void NavigateHyperlinkExecute(RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.AbsoluteUri);
        }

        public override void OnClose()
        {
            base.OnClose();
            UpdateTimer?.Stop();
            UpdateTimer = null;
        }

        public override void OnRefresh()
        {
            base.OnRefresh();
            UpdateTimer.Interval = Settings.RefreshInterval;
            if (_lastFeedUrl != Settings.RssFeedUrl)
                UpdateFeed();
        }
    }
}