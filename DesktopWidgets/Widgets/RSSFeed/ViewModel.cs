using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
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
            UpdateTimer.Start();
            if (string.IsNullOrWhiteSpace(Settings.RssFeedUrl))
            {
                ShowHelp = true;
            }
            else
            {
                UpdateFeed();
            }
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

        private void UpdateFeed()
        {
            var reader = XmlReader.Create(Settings.RssFeedUrl);
            var feed = SyndicationFeed.Load(reader);
            reader.Close();
            if (feed?.Items == null)
                return;
            FeedItems.Clear();
            foreach (var item in feed.Items)
            {
                FeedItems.Add(new FeedItem(item.Title.Text, item.Links.FirstOrDefault()?.Uri?.AbsoluteUri));
                if (FeedItems.Count >= Settings.MaxHeadlines && Settings.MaxHeadlines > 0)
                    break;
            }
        }

        private void NavigateHyperlinkExecute(RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.AbsoluteUri);
        }
    }
}