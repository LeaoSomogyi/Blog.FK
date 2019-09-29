using Blog.FK.Web.ViewModels;
using FluentValidation;

namespace Blog.FK.Web.Validators
{
    public class BlogPostViewModelValidator : AbstractValidator<BlogPostViewModel>
    {
        public BlogPostViewModelValidator()
        {
            #region "  Not Null / Not Empty  "

            RuleFor(b => b.Title)
                .Must(t => t != null && !string.IsNullOrEmpty(t))
                .WithMessage("Por favor, informe o título do post")
                .WithErrorCode("400");

            RuleFor(b => b.ShortDescription)
                .Must(s => s != null && !string.IsNullOrEmpty(s))
                .WithMessage("Por favor, informe a descrição do post")
                .WithErrorCode("400");

            RuleFor(b => b.Content)
                .Must(c => c != null && !string.IsNullOrEmpty(c))
                .WithMessage("Por favor, informe o conteúdo do post")
                .WithErrorCode("400");

            #endregion

            #region "  Max Length / Min Length  "

            RuleFor(b => b.Title)
                .MaximumLength(256)
                .WithMessage("O campo título deve conter no máximo 256 caracteres")
                .WithErrorCode("400");

            RuleFor(b => b.ShortDescription)
                .MaximumLength(2048)
                .WithMessage("O campo descrição deve conter no máximo 2048 caracteres")
                .WithErrorCode("400");

            RuleFor(b => b.Title)
                .MinimumLength(5)
                .WithMessage("O campo título deve conter no mínimo 5 caracteres")
                .WithErrorCode("400");

            RuleFor(b => b.ShortDescription)
                .MinimumLength(10)
                .WithMessage("O campo descrição deve conter no mínimo 10 caracteres")
                .WithErrorCode("400");

            #endregion
        }
    }
}
