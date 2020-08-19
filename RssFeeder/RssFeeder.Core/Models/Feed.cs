using System;

namespace RssFeeder.Core.Models
{
    public class Feed
    {
        public Guid Id { get; }
        public String Title { get; }
        public String Description { get; }
        public DateTime? PublicationTime { get; }
        public String Link { get; }

        public Feed(String title, String description, DateTime publicationTime, String link)
        {
            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            PublicationTime = publicationTime;
            Link = link;
        }

        public Boolean IsFilled()
        {
            return !(Title is null || 
                     Description is null || 
                     PublicationTime is null || 
                     Link is null);
        }

    }
}