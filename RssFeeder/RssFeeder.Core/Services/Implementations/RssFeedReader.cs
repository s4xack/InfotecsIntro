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
            XmlNode chanel = root?.SelectSingleNode("chanel");
            if (chanel is null)
                throw new NotSupportedException();

            XmlNodeList items = chanel.SelectNodes("item");
            if (items is null)
                throw new NotSupportedException();

            List<Feed> feed = new List<Feed>();
            foreach (XmlElement item in items)
            {
                feed.Add(GetRssFeed(item));
            }

            return feed;
        }

        private Feed GetRssFeed(XmlNode node)
        {
            Feed feed = new Feed
            (
                node.SelectSingleNode("title")?.InnerText,
                node.SelectSingleNode("description")?.InnerText,
                DateTime.Parse(node.SelectSingleNode("pubdate")?.InnerText),
                node.SelectSingleNode("link")?.InnerText
            );
            return feed;
        }
    }
}