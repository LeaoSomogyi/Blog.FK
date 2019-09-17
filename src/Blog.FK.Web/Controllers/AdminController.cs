using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Blog.FK.Application.Interfaces;
using Blog.FK.Domain.Entities;
using Blog.FK.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.FK.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUserApplication _userApp;

        public AdminController(IMapper mapper, IUserApplication userApp)
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

            var token = await _userApp.AuthenticateAsync(_user);

            if (token == null)
            {
                TempData["msg"] = "Usuário ou senha incorretos.";
                return RedirectToAction("Login");
            }
            else
            {
                return RedirectToAction("Home/Index");
            }
        }
    }
}