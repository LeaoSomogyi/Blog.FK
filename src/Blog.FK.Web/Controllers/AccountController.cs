using AutoMapper;
using Blog.FK.Application.Interfaces;
using Blog.FK.Domain.Entities;
using Blog.FK.Web.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Blog.FK.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUserApplication _userApp;

        public AccountController(IMapper mapper, IUserApplication userApp)
        {
            _mapper = mapper;
            _userApp = userApp;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserViewModel user)
        {
            var _user = _mapper.Map<User>(user);

            var claimsPrincipal = await _userApp.AuthenticateAsync(_user);

            if (claimsPrincipal == null)
            {
                TempData["msg"] = "Usuário ou senha incorretos.";

                TempData.Keep("msg");

                return RedirectToAction("Login");
            }
            else
            {
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal,
                     new AuthenticationProperties
                     {
                         IsPersistent = true,
                         ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                     });

                return RedirectToAction("Create", "Blog");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }
    }
}