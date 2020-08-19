using System;
using System.Collections.Generic;

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
    }
}