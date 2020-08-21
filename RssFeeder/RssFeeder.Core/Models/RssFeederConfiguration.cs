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
            RefreshTimeout = Int32.TryParse(ParseNodeText(node, RefreshTimeoutXmlSelector), out Int32 refreshTimeout)
                ? refreshTimeout
                : 60;
            IsFormatting = Boolean.TryParse(ParseNodeText(node, IsFormattingXmlSelector), out Boolean isFormatting) &&
                           isFormatting;

            Sources = new List<RssSource>();
            
            var sourceNodes = node.SelectSingleNode(SourceContainerXmlSelector)?.SelectNodes(SourceXmlSelector);
            if (sourceNodes != null)
            {
                foreach (XmlNode sourceNode in sourceNodes)
                {
                    String link = ParseNodeText(sourceNode, LinkXmlSelector);
                    Boolean isActive =
                        Boolean.TryParse(ParseNodeText(node, IsActiveXmlSelector), out isActive) ||
                        isFormatting;
                    if (link != null)
                        Sources.Add(new RssSource(link, isActive));
                }
            }

            if (Sources.Count == 0)
                Sources.Add(new RssSource("https://habr.com/ru/rss/interesting", true));
        }

        private String ParseNodeText(XmlNode node, string selector)
        {
            return node.SelectSingleNode(selector)?.InnerText;
        }

        private const String RefreshTimeoutXmlSelector = "RefreshTimeout";
        private const String IsFormattingXmlSelector = "IsFormatting";
        private const String SourceContainerXmlSelector = "Sources";
        private const String SourceXmlSelector = "Source";
        private const String LinkXmlSelector = "Link";
        private const String IsActiveXmlSelector = "IsActive";
    }
}