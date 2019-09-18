using Blog.FK.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.FK.Domain.Interfaces
{
    public interface IBlogPostRepository : IBaseRepository<BlogPost>
    {
        /// <summary>
        /// Load more Blog Posts based on last list size, always 3 in 3
        /// </summary>
        /// <param name="actualListSize">Actual size of the blog post list</param>
        /// <returns>More blog Posts</returns>
        Task<IEnumerable<BlogPost>> GetMoreBlogPostsAsync(int actualListSize);
    }
}
