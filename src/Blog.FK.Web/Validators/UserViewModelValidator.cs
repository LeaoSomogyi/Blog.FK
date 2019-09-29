using Blog.FK.Web.ViewModels;
using FluentValidation;

namespace Blog.FK.Web.Validators
{
    public class UserViewModelValidator : AbstractValidator<UserViewModel>
    {
        public UserViewModelValidator()
        {
            #region "  Not Null / Not Empty  "

            RuleFor(u => u.Email)
                .Must(e => e != null && !string.IsNullOrEmpty(e))
                .WithMessage("Por favor, informe o seu e-mail")
                .WithErrorCode("400");

            RuleFor(u => u.Password)
                .Must(p => p != null && !string.IsNullOrEmpty(p))
                .WithMessage("Por favor, informe a sua senha")
                .WithErrorCode("400");

            #endregion

            #region "  Max Length / Min Length  "

            RuleFor(u => u.Email)
                .MaximumLength(256)
                .WithMessage("O campo e-mail deve conter no máximo 256 caracteres")
                .WithErrorCode("400");

            RuleFor(u => u.Password)
                .MaximumLength(2048)
                .WithMessage("O campo senha deve conter no máximo 2048 caracteres")
                .WithErrorCode("400");

            RuleFor(u => u.Email)
                .MinimumLength(5)
                .WithMessage("O campo e-mail deve conter no mínimo 5 caracteres")
                .WithErrorCode("400");

            RuleFor(u => u.Password)
                .MinimumLength(4)
                .WithMessage("O campo senha deve conter no mínimo 4 caracteres")
                .WithErrorCode("400");

            #endregion
        }
    }
}
