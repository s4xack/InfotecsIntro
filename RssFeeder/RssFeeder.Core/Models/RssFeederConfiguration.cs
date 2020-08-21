using System;
using System.Collections.Generic;
using System.Xml;

namespace RssFeeder.Core.Models
{
    
    public class RssFeederConfiguration
    {
        public List<RssSource> Sources { get; }
        public Int32 RefreshTimeout { get; set; }
        public Boolean IsFormatting { get; set; }

        public RssFeederConfiguration(List<RssSource> sources, Int32 refreshTimeout, Boolean isFormatting)
        {
            Sources = sources;
            RefreshTimeout = refreshTimeout;
            IsFormatting = isFormatting;
        }

        public RssFeederConfiguration(XmlNode node)
        {
            RefreshTimeout = Int32.Parse(node.SelectSingleNode(nameof(RefreshTimeout))?.InnerText ?? "60");
            IsFormatting = Boolean.Parse(node.SelectSingleNode(nameof(IsFormatting))?.InnerText ?? "false");
            Sources = new List<RssSource>();
            
            var sourceNodes = node.SelectSingleNode("Sources")?.SelectNodes("Source");
            if (sourceNodes != null)
            {
                foreach (XmlNode sourceNode in sourceNodes)
                {
                    RssSource rssSource = new RssSource(sourceNode.SelectSingleNode("Link")?.InnerText,
                        Boolean.Parse(sourceNode.SelectSingleNode("IsActive")?.InnerText ?? "false"));
                    if (rssSource.Link != null)
                        Sources.Add(rssSource);
                }
            }

            if (Sources.Count == 0)
                Sources.Add(new RssSource("https://habr.com/ru/rss/interesting", true));
        }
    }
}