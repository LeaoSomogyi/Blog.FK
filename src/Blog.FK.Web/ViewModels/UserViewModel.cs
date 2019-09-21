using System;
using System.ComponentModel.DataAnnotations;

namespace Blog.FK.Web.ViewModels
{
    public class UserViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Nome")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "E-mail")]
        [Required]
        public string Email { get; set; }

        [Display(Name = "Senha")]
        [Required]
        public string Password { get; set; }

        public DateTime CreatedAt { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public DateTime UpdatedAt { get; set; }

        public UserViewModel() { }
    }
}
