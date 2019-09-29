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
            var blogPost = TestHelper.GetBlogPostViewModel();

            var response = await _blogController.SavePost(blogPost);

            Assert.True(response is LocalRedirectResult);
        }

        #endregion

        #region "  NOk  "

        //To be implemented

        #endregion

        #region "  Theory  "

        //To be implemented

        #endregion
    }
}
