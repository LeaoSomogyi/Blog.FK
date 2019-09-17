using Blog.FK.Domain.Entities;
using Blog.FK.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blog.FK.Infra.Repositories
{
    public class BlogPostRepository : BaseRepository<BlogPost>, IBlogPostRepository
    {
        public BlogPostRepository(DbContext dbContext) : base(dbContext) { }
    }
}
