using Blog.FK.Domain.Extensions;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;

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

        public UserViewModel UserViewModel { get; set; }

        public BlogPostViewModel() { }

        /// <summary>
        /// Set current authenticated user as Author
        /// </summary>
        /// <param name="claimsPrincipal">Current authenticated user</param>
        /// <returns>Blog.FK.Web.ViewModels.BlogPostViewModel</returns>
        public BlogPostViewModel SetAuthor(ClaimsPrincipal claimsPrincipal)
        {
            UserViewModel = new UserViewModel
            {
                Id = Guid.Parse(claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Hash).Value),
                Name = claimsPrincipal.Identity.Name,
                Email = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value
            };

            return this;
        }
    }
}
