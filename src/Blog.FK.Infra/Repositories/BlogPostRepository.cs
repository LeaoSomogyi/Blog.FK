using Blog.FK.Domain.Entities;
using Blog.FK.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.FK.Infra.Repositories
{
    public class BlogPostRepository : BaseRepository<BlogPost>, IBlogPostRepository
    {
        public BlogPostRepository(DbContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<BlogPost>> GetMoreBlogPostsAsync(int actualListSize)
        {
            var blogPosts = await DbContext.Set<BlogPost>().ToListAsync();

            return blogPosts.OrderByDescending(b => b.CreatedAt).Skip(actualListSize).Take(3);
        }
    }
}
