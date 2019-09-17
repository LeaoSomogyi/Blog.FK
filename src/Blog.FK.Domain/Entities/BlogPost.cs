using Blog.FK.Domain.Extensions;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.FK.Domain.Entities
{
    public class BlogPost
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string ShortDescription { get; set; }

        public string Link
        {
            get
            {
                return ShortDescription.UrlFriendly();
            }
        }

        [NotMapped]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public BlogPost() { }
    }
}
