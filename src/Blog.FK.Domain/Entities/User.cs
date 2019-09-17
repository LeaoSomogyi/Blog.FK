using System;
using System.Collections.Generic;
using System.Text;

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

        public User() { }
    }
}
