namespace DesktopWidgets.Widgets.RSSFeed
{
    public class FeedItem
    {
        public FeedItem(string title, string hyperlink, string publishDateTime)
        {
            Title = title;
            Hyperlink = hyperlink;
            PublishDate = publishDateTime;
        }

        public string Title { get; set; }
        public string Hyperlink { get; set; }
        public string PublishDate { get; set; }
    }
}