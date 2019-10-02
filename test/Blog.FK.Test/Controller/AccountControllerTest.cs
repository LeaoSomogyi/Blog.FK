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
using System.Linq;
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
        public void Create_Ok()
        {
            //Act
            var response = _accountController.Create();

            //Assert
            Assert.True(response.CastActionResult<ViewResult>() != null);
        }

        [Fact]
        public void LoginView_Ok()
        {
            //Act
            var response = _accountController.Login();

            //Assert
            Assert.True(response.CastActionResult<ViewResult>() != null);
        }

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
        public async Task Logout_Ok()
        {
            //Act
            var response = await _accountController.Logout();

            //Assert
            Assert.True(response is LocalRedirectResult);
            Assert.Empty(_accountController.HttpContext.User.Claims);
        }

        [Fact]
        public async Task SaveUser_Ok()
        {
            //Arrange
            var user = TestHelper.GetUserViewModel();

            //Act
            var response = await _accountController.SaveUser(user);

            //Need to force Dispose to save new User and use on further tests
            _accountController.Dispose();

            //Assert
            Assert.True(response is LocalRedirectResult);
            Assert.True(_accountController.TempData["msg"] != null);
            Assert.True(_accountController.TempData["error"] == null);
        }

        [Fact]
        public async Task List_Ok()
        {
            //Act
            var response = await _accountController.List();

            //Assert
            Assert.True(response is ViewResult);
            Assert.True(response.GetModelFromViewResultResponse<IEnumerable<UserViewModel>>() != null);
        }

        [Fact]
        public async Task Edit_Ok()
        {
            //Arrange
            var response = await _accountController.List();

            var users = response.GetModelFromViewResultResponse<IEnumerable<UserViewModel>>();

            var user = users.FirstOrDefault();

            //Act
            var editResponse = await _accountController.Edit(user.Id);

            var editUser = editResponse.GetModelFromViewResultResponse<UserViewModel>();

            //Assert
            Assert.True(editResponse is ViewResult);
            Assert.True(editUser != null);
            Assert.Equal(user.Id, editUser.Id);
        }

        [Fact]
        public async Task Remove_Ok()
        {
            //Arrange
            var response = await _accountController.List();

            var users = response.GetModelFromViewResultResponse<IEnumerable<UserViewModel>>();

            var user = users.LastOrDefault();

            //Act
            var removeResponse = await _accountController.Remove(user.Id);

            //Need to force Dispose to commit delete transaction
            _accountController.Dispose();

            var listResponse = await _accountController.List();

            var newUserList = listResponse.GetModelFromViewResultResponse<IEnumerable<UserViewModel>>();

            //Assert
            Assert.True(removeResponse is LocalRedirectResult);
            Assert.Empty(newUserList);
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
