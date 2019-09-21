using Blog.FK.Domain.Extensions;
using System;
using System.ComponentModel.DataAnnotations;

namespace Blog.FK.Web.ViewModels
{
    public class BlogPostViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Título")]
        [Required]
        public string Title { get; set; }

        [Display(Name = "Descrição")]
        [Required]
        public string ShortDescription { get; set; }

        public string Link
        {
            get
            {
                return ShortDescription.UrlFriendly();
            }
        }

        [Display(Name = "Conteúdo do Post")]
        [Required]
        public string Content { get; set; }
        
        public DateTime CreatedAt { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public DateTime UpdatedAt { get; set; }

        public BlogPostViewModel() { }
    }
}
