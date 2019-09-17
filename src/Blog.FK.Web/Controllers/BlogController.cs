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
    public class BlogController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IBlogPostApplication _blogApp;

        public BlogController(IMapper mapper, IBlogPostApplication blogApp)
        {
            _mapper = mapper;
            _blogApp = blogApp;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> GetLatestPosts()
        {
            var posts = await _blogApp.GetAllAsync();

            return Json(posts.OrderByDescending(p => p.CreatedAt));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SavePost(BlogPostViewModel blogPost)
        {
            var _blogPost = _mapper.Map<BlogPost>(blogPost);

            await _blogApp.AddAsync(_blogPost);

            TempData["msg"] = "Post cadastrado com sucesso!";

            return Redirect("Create");
        }

        [HttpGet]
        public async Task<ContentResult> LoadBlogPost(Guid id)
        {
            var blogPost = await _blogApp.FindAsync(id);

            return Content(blogPost.Content);
        }
    }
}