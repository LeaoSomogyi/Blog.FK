using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Blog.FK.Web.ViewModels
{
    public class MobileNativeViewModel
    {
        [Display(Name = "Foto")]
        public IFormFile Photo { get; set; }

        [Display(Name = "Vídeo")]
        public IFormFile Video { get; set; }

        [Display(Name = "Áudio")]
        public IFormFile Audio { get; set; }
    }
}
