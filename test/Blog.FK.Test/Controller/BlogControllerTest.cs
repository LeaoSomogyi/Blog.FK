using AutoMapper;
using Blog.FK.Application.Interfaces;
using Blog.FK.Test.Fixtures;
using Blog.FK.Test.Utils;
using Blog.FK.Web.Controllers;
using Blog.FK.Web.ViewModels;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Blog.FK.Test.Controller
{
    public class BlogControllerTest : BaseTest
    {
        #region

        private readonly BlogController _blogController;

        #endregion

        #region "  Constructor  "

        public BlogControllerTest(TestDatabaseFixture testDatabaseFixture) : base(testDatabaseFixture)
        {
            var mapperService = Server.GetService<IMapper>();
            var blogApp = Server.GetService<IBlogPostApplication>();
            var validator = Server.GetService<IValidator<BlogPostViewModel>>();
            var tempDataProvider = Server.GetService<ITempDataProvider>();

            _blogController = new BlogController(mapperService, blogApp, validator)
            {
                TempData = new TempDataDictionary(new DefaultHttpContext(), tempDataProvider)
            };
        }

        #endregion

        #region "  Ok  "

        [Fact]
        public async Task SaveBlogPost_Ok()
        {
            //Arrange
            var blogPost = TestHelper.GetBlogPostViewModel();

            //Act
            var response = await _blogController.SavePost(blogPost);
            
            //Assert  
            Assert.True(response is LocalRedirectResult);
            Assert.True(_blogController.TempData["msg"] != null);
            Assert.True(_blogController.TempData["error"] == null);
        }

        #endregion

        #region "  NOk  "

        [Theory]
        [MemberData(nameof(Invalid_BlogPosts))]
        public async Task SaveBlogPost_NOk(BlogPostViewModel blogPostViewModel)
        {
            //Act
            var response = await _blogController.SavePost(blogPostViewModel);

            //Assert
            Assert.True(response is LocalRedirectResult);
            Assert.True(_blogController.TempData["error"] != null);
        }

        #endregion

        #region "  Theory  "

        public static IEnumerable<object[]> Invalid_BlogPosts()
        {
            return new List<object[]>()
            {
                new object[] { TestHelper.GetBlogPostViewModel().Moq((b) => b.Title = null) },
                new object[] { TestHelper.GetBlogPostViewModel().Moq((b) => b.ShortDescription = null) },
                new object[] { TestHelper.GetBlogPostViewModel().Moq((b) => b.Content = null) },
                new object[] { TestHelper.GetBlogPostViewModel().Moq((b) => b.Title = string.Empty.GenerateRandomCharacters(257)) },
                new object[] { TestHelper.GetBlogPostViewModel().Moq((b) => b.ShortDescription = string.Empty.GenerateRandomCharacters(2049)) },
                new object[] { TestHelper.GetBlogPostViewModel().Moq((b) => b.Title = string.Empty.GenerateRandomCharacters(4)) },
                new object[] { TestHelper.GetBlogPostViewModel().Moq((b) => b.ShortDescription = string.Empty.GenerateRandomCharacters(9)) }
            };
        }

        #endregion
    }
}
