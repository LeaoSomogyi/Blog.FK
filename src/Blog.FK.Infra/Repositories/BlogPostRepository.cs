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

        public override async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            try
            {
                return await DbContext.Set<BlogPost>().Include(b => b.User).ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<BlogPost>> GetMoreBlogPostsAsync(int actualListSize)
        {
            try
            {
                var blogPosts = await DbContext.Set<BlogPost>().ToListAsync();

                return blogPosts.OrderByDescending(b => b.CreatedAt).Skip(actualListSize).Take(3);
            }
            catch
            {
                throw;
            }
        }
    }
}
