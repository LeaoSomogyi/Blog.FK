using AutoMapper;
using Blog.FK.Application.Interfaces;
using Blog.FK.Domain.Entities;
using Blog.FK.Web.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        #region "  Admin Actions  "

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SaveUser(UserViewModel userViewModel)
        {
            var user = _mapper.Map<User>(userViewModel);

            await _userApp.AddAsync(user);

            TempData["msg"] = "Usuário cadastrado com sucesso!";

            TempData.Keep("msg");

            return Redirect("Create");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> List()
        {
            var users = await _userApp.GetAllAsync();

            var _users = _mapper.Map<IEnumerable<UserViewModel>>(users.OrderByDescending(u => u.CreatedAt));

            return View(_users);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var user = await _userApp.FindAsync(id);

            var _user = _mapper.Map<UserViewModel>(user);

            return View("Create", _user);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Remove(Guid id)
        {
            var user = await _userApp.FindAsync(id);

            _userApp.Remove(user);

            TempData["msg"] = "Usuário removido com sucesso!";

            TempData.Keep("msg");

            return RedirectToAction("List");
        }

        #endregion
    }
}