using System;
using System.Collections.ObjectModel;
using System.ServiceModel.Syndication;

namespace DesktopWidgets.Widgets.RSSFeed
{
    public class FeedItem
    {
        public FeedItem(string title, string hyperlink, DateTime publishDateTime,
            Collection<SyndicationCategory> categories)
        {
            Title = title;
            Hyperlink = hyperlink;
            PublishDate = publishDateTime;
            Categories = categories;
        }

        public string Title { get; set; }
        public string Hyperlink { get; set; }
        public DateTime PublishDate { get; set; }
        public Collection<SyndicationCategory> Categories { get; set; }
    }
}