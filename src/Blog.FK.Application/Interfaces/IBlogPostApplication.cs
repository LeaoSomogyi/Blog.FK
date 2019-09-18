using Blog.FK.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.FK.Application.Interfaces
{
    public interface IBlogPostApplication : IBaseApplication<BlogPost>
    {
        /// <summary>
        /// Load more Blog Posts based on last list size
        /// </summary>
        /// <param name="actualListSize">Actual size of the blog post list</param>
        /// <returns>More blog Posts</returns>
        Task<IEnumerable<BlogPost>> GetMoreBlogPostsAsync(int actualListSize);
    }
}
