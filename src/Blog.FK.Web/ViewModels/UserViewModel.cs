using System;
using System.ComponentModel.DataAnnotations;

namespace Blog.FK.Web.ViewModels
{
    public class UserViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        [Display(Name = "E-mail")]
        [Required]
        public string Email { get; set; }

        [Display(Name = "Senha")]
        [Required]
        public string Password { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public UserViewModel() { }
    }
}
