using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Xml;
using RssFeeder.Core.Models;
using RssFeeder.Core.Services.Abstractions;

namespace RssFeeder.Core.Services.Implementations
{
    public class RssFeedReader : IRssFeedReader
    {
        public List<Feed> GetFeeds(List<RssSource> sources)
        {
            return sources.Where(s => s.IsActive)
                .Select(s => s.Link)
                .Select(ParseRss)
                .SelectMany(l => l)
                .ToList();
        }

        private List<Feed> ParseRss(String rssSourceLink)
        {
            XmlDocument document = new XmlDocument();
            document.Load(rssSourceLink);

            XmlElement root = document.DocumentElement;
            XmlNode chanel = root?.SelectSingleNode("channel");
            if (chanel is null)
                throw new NotSupportedException();

            XmlNodeList items = chanel.SelectNodes("item");
            if (items is null)
                throw new NotSupportedException();

            List<Feed> feeds = new List<Feed>();
            foreach (XmlElement item in items)
            {
                feeds.Add(GetRssFeed(item));
            }

            return feeds.Where(f => f.IsFilled()).ToList();
        }

        private Feed GetRssFeed(XmlNode node)
        {
            String title = node.SelectSingleNode("title")?.InnerText;
            String description = node.SelectSingleNode("description")?.InnerText;
            DateTime publicationTime = DateTime.Parse(node.SelectSingleNode("pubDate")?.InnerText);
            var link = node.SelectSingleNode("link")?.InnerText;
            Feed feed = new Feed
            (
                title,
                description,
                publicationTime,
                link
            );
            return feed;
        }
    }
}