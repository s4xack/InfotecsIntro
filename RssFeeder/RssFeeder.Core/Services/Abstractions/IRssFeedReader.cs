using System.Collections.Generic;
using RssFeeder.Core.Models;

namespace RssFeeder.Core.Services.Abstractions
{
    public interface IRssFeedReader
    {
        List<Feed> GetFeeds(List<RssSource> sources);
    }
}