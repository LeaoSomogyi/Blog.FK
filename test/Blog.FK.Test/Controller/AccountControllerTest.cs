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
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Blog.FK.Test.Controller
{
    public class AccountControllerTest : BaseTest
    {
        #region "  Properties  "

        private readonly AccountController _accountController;

        #endregion

        #region "  Constructor  "

        public AccountControllerTest(TestDatabaseFixture testDatabaseFixture) : base(testDatabaseFixture)
        {
            var mapperService = Server.GetService<IMapper>();
            var userApp = Server.GetService<IUserApplication>();
            var validator = Server.GetService<IValidator<UserViewModel>>();
            var tempDataProvider = Server.GetService<ITempDataProvider>();

            _accountController = new AccountController(mapperService, userApp, validator)
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
        public async Task Login_Ok()
        {
            //Arrange
            var user = TestHelper.GetLoginUserViewModel();

            //Act
            var response = await _accountController.Login(user);

            //Assert
            Assert.True(response is LocalRedirectResult);
            Assert.True(_accountController.HttpContext.User != null);
        }

        [Fact]
        public async Task SaveUser_Ok()
        {
            //Arrange
            var user = TestHelper.GetUserViewModel();

            //Act
            var response = await _accountController.SaveUser(user);

            //Assert
            Assert.True(response is LocalRedirectResult);
            Assert.True(_accountController.TempData["msg"] != null);
            Assert.True(_accountController.TempData["error"] == null);
        }

        #endregion

        #region "  NOk  "

        [Theory]
        [MemberData(nameof(Invalid_LoginData))]
        public async Task Login_NOk(UserViewModel userViewModel)
        {
            //Act
            var response = await _accountController.Login(userViewModel);

            //Assert
            Assert.True(response is LocalRedirectResult);
            Assert.True(_accountController.TempData["error"] != null);
        }

        [Theory]
        [MemberData(nameof(Invalid_Users))]
        public async Task SaveUser_NOk(UserViewModel userViewModel)
        {
            //Act
            var response = await _accountController.SaveUser(userViewModel);

            //Assert
            Assert.True(response is LocalRedirectResult);
            Assert.True(_accountController.TempData["error"] != null);
        }

        #endregion

        #region "  Theory  "

        public static IEnumerable<object[]> Invalid_LoginData()
        {
            return new List<object[]>()
            {
                new object[] { TestHelper.GetLoginUserViewModel().Moq((b) => b.Email = null) },
                new object[] { TestHelper.GetLoginUserViewModel().Moq((b) => b.Password = null) },
                new object[] { TestHelper.GetLoginUserViewModel().Moq((b) => b.Email = string.Empty.GenerateRandomCharacters(257)) },
                new object[] { TestHelper.GetLoginUserViewModel().Moq((b) => b.Password = string.Empty.GenerateRandomCharacters(2049)) },
                new object[] { TestHelper.GetLoginUserViewModel().Moq((b) => b.Email = string.Empty.GenerateRandomCharacters(3)) },
                new object[] { TestHelper.GetLoginUserViewModel().Moq((b) => b.Password = string.Empty.GenerateRandomCharacters(3)) }
            };
        }

        public static IEnumerable<object[]> Invalid_Users()
        {
            return new List<object[]>()
            {
                new object[] { TestHelper.GetLoginUserViewModel().Moq((b) => b.Email = null) },
                new object[] { TestHelper.GetLoginUserViewModel().Moq((b) => b.Password = null) },
                new object[] { TestHelper.GetLoginUserViewModel().Moq((b) => b.Email = string.Empty.GenerateRandomCharacters(257)) },
                new object[] { TestHelper.GetLoginUserViewModel().Moq((b) => b.Password = string.Empty.GenerateRandomCharacters(2049)) },
                new object[] { TestHelper.GetLoginUserViewModel().Moq((b) => b.Email = string.Empty.GenerateRandomCharacters(3)) },
                new object[] { TestHelper.GetLoginUserViewModel().Moq((b) => b.Password = string.Empty.GenerateRandomCharacters(4)) },
                new object[] { TestHelper.GetUserViewModel().Moq((b) => b.Name = null) },
                new object[] { TestHelper.GetUserViewModel().Moq((b) => b.Name = string.Empty.GenerateRandomCharacters(257)) },
                new object[] { TestHelper.GetUserViewModel().Moq((b) => b.Name = string.Empty.GenerateRandomCharacters(3)) }
            };
        }

        #endregion
    }
}
