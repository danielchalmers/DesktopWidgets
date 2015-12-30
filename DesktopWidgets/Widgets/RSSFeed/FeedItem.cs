namespace DesktopWidgets.Widgets.RSSFeed
{
    public class FeedItem
    {
        public FeedItem(string title, string hyperlink)
        {
            Title = title;
            Hyperlink = hyperlink;
        }

        public string Title { get; set; }
        public string Hyperlink { get; set; }
    }
}