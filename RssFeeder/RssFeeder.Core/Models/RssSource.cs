using System;

namespace RssFeeder.Core.Models
{
    public class RssSource
    {
        public String Link { get; }
        public Boolean IsActive { get; set; }

        public RssSource(String link)
        {
            Link = link;
            IsActive = true;
        }
    }
}