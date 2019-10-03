using Blog.FK.Application.Interfaces;
using Blog.FK.Domain.Entities;
using Blog.FK.Domain.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Blog.FK.Application
{
    public class BlogPostApplication : BaseApplication<BlogPost>, IBlogPostApplication
    {
        #region "  Services & Repositories  "

        private readonly IBlogPostRepository _blogPostRepository;
        private readonly IHostingEnvironment _environment;
        private readonly string _postsPath;

        #endregion

        #region "  Constructors  "

        public BlogPostApplication(IBlogPostRepository blogPostRepository, IHostingEnvironment environment,
            IConfiguration configuration) : base(blogPostRepository)
        {
            _blogPostRepository = blogPostRepository;
            _environment = environment;
            _postsPath = $"{_environment.ContentRootPath}{configuration["PostsPath"]}";
        }

        #endregion

        #region "  Overrides from BaseApplication  "

        public override async Task<BlogPost> AddAsync(BlogPost entity)
        {
            var blogPost = await base.AddAsync(entity);

            if (!Directory.Exists(_postsPath))
            {
                Directory.CreateDirectory(_postsPath);
            }

            using (var fileStream = File.Create($"{_postsPath}/{blogPost.Id}_post.md"))
            {
                var bytes = new UTF8Encoding(true).GetBytes(blogPost.Content);

                fileStream.Write(bytes, 0, bytes.Length);
            }

            return blogPost;
        }

        public override async Task<BlogPost> FindAsync(Guid id)
        {
            var blogPost = await base.FindAsync(id);

            using (var streamReader = new StreamReader($"{_postsPath}/{blogPost.Id}_post.md"))
            {
                var content = await streamReader.ReadToEndAsync();

                blogPost.Content = content;
            }

            return blogPost;
        }

        public override void Remove(BlogPost entity)
        {
            string postPath = $"{_postsPath}/{entity.Id}_post.md";

            if (File.Exists(postPath))
            {
                File.Delete(postPath);
            }

            base.Remove(entity);
        }

        #endregion

        #region "  IBlogPostApplication  "

        public async Task<IEnumerable<BlogPost>> GetMoreBlogPostsAsync(int actualListSize)
        {
            return await _blogPostRepository.GetMoreBlogPostsAsync(actualListSize);
        }

        #endregion
    }
}
