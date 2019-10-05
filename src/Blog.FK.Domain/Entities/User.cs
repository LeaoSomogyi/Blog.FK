using System;
using System.Collections.Generic;

namespace Blog.FK.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public ICollection<BlogPost> BlogPosts { get; set; }

        public User() { }
    }
}
