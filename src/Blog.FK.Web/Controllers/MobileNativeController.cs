using Blog.FK.Web.Extensions;
using Blog.FK.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Blog.FK.Web.Controllers
{
    public class MobileNativeController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public MobileNativeController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        [DisableRequestSizeLimit]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        [AllowAnonymous]
        public async Task<IActionResult> Photo(MobileNativeViewModel mobileNativeViewModel)
        {
            if (mobileNativeViewModel?.Photo == null)
            {
                TempData["photoError"] = "Para visualizar uma foto, é necessário escolhê-la primeiro.";
                TempData.Keep();

                return View(nameof(Index));
            }
            else
            {
                var photoBase64 = await mobileNativeViewModel.Photo.ConvertToBase64();

                return View("Photo", photoBase64);
            }
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        [AllowAnonymous]
        public async Task<IActionResult> Video(MobileNativeViewModel mobileNativeViewModel)
        {
            if (mobileNativeViewModel?.Video == null)
            {
                TempData["videoError"] = "Para visualizar um vídeo, é necessário escolhê-lo primeiro.";
                TempData.Keep();

                return View(nameof(Index));
            }
            else
            {
                var video = mobileNativeViewModel.Video;

                var videoBytes = await video.GetRawBytes();

                var guid = Guid.NewGuid();

                HttpContext.Session.SetString("video", guid.ToString());

                using (var fs = System.IO.File.Create($"{_hostingEnvironment.WebRootPath}/Videos/{guid}.mp4"))
                {
                    await fs.WriteAsync(videoBytes);
                }

                return View("Video", $"{guid.ToString()}.mp4");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult CleanVideo(string id) 
        {
            var path = $"{_hostingEnvironment.WebRootPath}\\Videos\\{id}";

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            return LocalRedirect("/MobileNative/Index");
        }

        [HttpPost]
        [AllowAnonymous]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Audio(MobileNativeViewModel mobileNativeViewModel)
        {
            if (mobileNativeViewModel?.Audio == null)
            {
                TempData["audioError"] = "Para visualizar um áudio, é necessário escolhê-lo primeiro.";
                TempData.Keep();

                return View(nameof(Index));
            }
            else
            {
                var audio = mobileNativeViewModel.Audio;

                var audioBytes = await audio.GetRawBytes();

                var file = File(audioBytes, "application/octet-stream");

                return View(file);
            }
        }

        [HttpGet]
        public IActionResult Punch() 
        {
            return View();
        }
    }
}