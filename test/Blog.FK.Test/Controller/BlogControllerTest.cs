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
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Blog.FK.Test.Controller
{
    public class BlogControllerTest : BaseTest
    {
        #region "  Properties  "

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
                TempData = new TempDataDictionary(new DefaultHttpContext(), tempDataProvider),
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        RequestServices = Server.Host.Services
                    }
                }
            };
        }

        #endregion

        #region "  Ok  "

        [Fact]
        public void Index_Ok()
        {
            //Act
            var response = _blogController.Index();

            //Assert
            Assert.True(response.CastActionResult<ViewResult>() != null);
        }

        [Fact]
        public void Create_Ok()
        {
            //Act
            var response = _blogController.Create();

            //Assert
            Assert.True(response.CastActionResult<ViewResult>() != null);
        }

        [Fact]
        public void Error_Ok()
        {
            //Act
            var response = _blogController.Error();

            var error = response.GetModelFromViewResultResponse<ErrorViewModel>();

            //Assert
            Assert.True(response is ViewResult);
            Assert.NotNull(error);
        }

        [Fact]
        public async Task GetLatestPosts_Ok()
        {
            //Act
            var jsonResponse = await _blogController.GetLatestPosts();

            var blogPosts = jsonResponse.CastJsonResult<IEnumerable<BlogPostViewModel>>();

            //Assert
            Assert.True(jsonResponse is JsonResult);
            Assert.NotNull(blogPosts);
        }

        [Fact]
        public async Task LoadBlogPost_Ok()
        {
            //Arrange
            var response = await _blogController.List();

            var blogPosts = response.GetModelFromViewResultResponse<IEnumerable<BlogPostViewModel>>();

            var blogPost = blogPosts.FirstOrDefault();

            //Act
            var loadResponse = await _blogController.LoadBlogPost(blogPost.Id);

            //Assert
            Assert.True(loadResponse is ContentResult);
            Assert.True(loadResponse.CastActionResult<ContentResult>().Content != null);
        }

        [Fact]
        public async Task GetMoreBlogPosts_Ok()
        {
            //Act
            var response = await _blogController.GetMoreBlogPosts(1);

            var blogPosts = response.CastJsonResult<IEnumerable<BlogPostViewModel>>();

            //Assert
            Assert.True(response is JsonResult);
            Assert.NotNull(blogPosts);
        }

        [Fact]
        public async Task SaveBlogPost_Ok()
        {
            //Arrange
            var blogPost = TestHelper.GetBlogPostViewModel();

            //Act
            var response = await _blogController.SavePost(blogPost);

            //Need to Dispose to save new Blog and use on further tests
            _blogController.Dispose();

            //Assert  
            Assert.True(response is LocalRedirectResult);
            Assert.True(_blogController.TempData["msg"] != null);
            Assert.True(_blogController.TempData["error"] == null);
        }

        [Fact]
        public async Task List_Ok()
        {
            //Act
            var response = await _blogController.List();

            //Assert
            Assert.True(response is ViewResult);
            Assert.True(response.GetModelFromViewResultResponse<IEnumerable<BlogPostViewModel>>() != null);
        }

        [Fact]
        public async Task Edit_Ok()
        {
            //Arrange
            var _blogPost = TestHelper.GetBlogPostViewModel();

            await _blogController.SavePost(_blogPost);

            //Need to Dispose to save new Blog and use on further tests
            _blogController.Dispose();

            var response = await _blogController.List();

            var blogPosts = response.GetModelFromViewResultResponse<IEnumerable<BlogPostViewModel>>();

            var blogPost = blogPosts.FirstOrDefault();

            //Act
            var editResponse = await _blogController.Edit(blogPost.Id);

            var editBlogPost = editResponse.GetModelFromViewResultResponse<BlogPostViewModel>();

            //Assert
            Assert.True(editResponse is ViewResult);
            Assert.True(editBlogPost != null);
            Assert.Equal(blogPost.Id, editBlogPost.Id);
        }

        [Fact]
        public async Task Remove_Ok()
        {
            //Arrange
            var response = await _blogController.List();

            var blogPosts = response.GetModelFromViewResultResponse<IEnumerable<BlogPostViewModel>>();

            var blogPost = blogPosts.LastOrDefault();

            //Act
            var removeResponse = await _blogController.Remove(blogPost.Id);

            //Need to force Dispose to commit delete transaction
            _blogController.Dispose();

            var listResponse = await _blogController.List();

            var newBlogPostList = listResponse.GetModelFromViewResultResponse<IEnumerable<BlogPostViewModel>>();

            //Assert
            Assert.True(removeResponse is LocalRedirectResult);
            Assert.True(blogPosts.Count() != newBlogPostList.Count());
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
