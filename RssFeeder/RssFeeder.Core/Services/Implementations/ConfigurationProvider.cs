using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Xml;
using RssFeeder.Core.Models;

namespace RssFeeder.Core.Services.Implementations
{
    public class ConfigurationProvider
    {
        private readonly RssFeederConfiguration _config;

        public List<RssSource> Sources => _config.Sources.ToList();
        public Int32 RefreshTimeout => _config.RefreshTimeout;
        public Boolean IsFormatting => _config.IsFormatting;

        public ConfigurationProvider(RssFeederConfiguration config)
        {
            _config = config;
        }

        public Boolean ToggleFormatting()
        {
            _config.IsFormatting = !_config.IsFormatting;
            return true;
        }

        public Boolean UpdateRefreshTimeout(Int32 newTimeout)
        {
            if (newTimeout < 60)
                return false;
            
            _config.RefreshTimeout = newTimeout;
            return true;
        }

        public Boolean ToggleRssSource(String rssSourceLink)
        {
            RssSource source = _config.Sources.Find(s => s.Link == rssSourceLink);

            if (source is null)
                return false;

            source.IsActive = !source.IsActive;
            return true;
        }

        public Boolean AddRssSource(String rssSourceLink)
        {
            if (!IsValidRssSource(rssSourceLink))
                return false;

            _config.Sources.Add(new RssSource(rssSourceLink));
            return true;
        }

        public Boolean RemoveRssSource(String rssSourceLink)
        {
            RssSource source = _config.Sources.Find(s => s.Link == rssSourceLink);

            if (source is null)
                return false;

            _config.Sources.Remove(source);
            return true;
        }

        private static Boolean IsValidRssSource(String rssSourceLink)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(rssSourceLink);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}