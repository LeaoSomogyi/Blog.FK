using AutoMapper;
using Blog.FK.Application.Interfaces;
using Blog.FK.Domain.Entities;
using Blog.FK.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

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

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public async Task<JsonResult> GetLatestPosts()
        {
            var posts = await _blogApp.GetAllAsync();

            return Json(posts.OrderByDescending(p => p.CreatedAt).Take(3));
        }

        [HttpGet]
        public async Task<ContentResult> LoadBlogPost(Guid id)
        {
            var blogPost = await _blogApp.FindAsync(id);

            return Content(blogPost.Content);
        }

        [HttpGet]
        public async Task<JsonResult> GetMoreBlogPosts(int actualListSize)
        {
            var blogPosts = await _blogApp.GetMoreBlogPostsAsync(actualListSize);

            return Json(blogPosts);
        }

        #region "  Admin Actions  "

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var blogPost = await _blogApp.FindAsync(id);

            var _blogPost = _mapper.Map<BlogPostViewModel>(blogPost);

            return View("Create", _blogPost);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SavePost(BlogPostViewModel blogPost)
        {
            var _blogPost = _mapper.Map<BlogPost>(blogPost);

            await _blogApp.AddAsync(_blogPost);

            TempData["msg"] = "Post cadastrado com sucesso!";

            TempData.Keep("msg");

            return LocalRedirect("Create");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> List()
        {
            var _posts = await _blogApp.GetAllAsync();

            var posts = _mapper.Map<IEnumerable<BlogPostViewModel>>(_posts.OrderByDescending(p => p.UpdatedAt));

            return View(posts);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Remove(Guid id)
        {
            var blogPost = await _blogApp.FindAsync(id);

            _blogApp.Remove(blogPost);

            TempData["msg"] = "Post removido com sucesso!";

            TempData.Keep("msg");

            return RedirectToAction("List");
        }

        #endregion
    }
}