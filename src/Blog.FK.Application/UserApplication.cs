using Blog.FK.Application.Interfaces;
using Blog.FK.Domain.Entities;
using Blog.FK.Domain.Extensions;
using Blog.FK.Domain.Interfaces;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blog.FK.Application
{
    public class UserApplication : BaseApplication<User>, IUserApplication
    {
        #region "  Services & Repositories  "

        private readonly IUserRepository _userRepository;

        #endregion

        #region "  Constructors  "

        public UserApplication(IUserRepository userRepository) : base(userRepository)
        {
            _userRepository = userRepository;
        }

        #endregion

        #region "  IUserApplication  "

        public async Task<ClaimsPrincipal> AuthenticateAsync(User user)
        {
            var result = await _userRepository.SearchAsync(u => u.Email == user.Email && u.Password == user.Password.Cript());

            var _user = result.FirstOrDefault();

            if (_user != null)
            {
                var identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, _user.Name),
                    new Claim(ClaimTypes.Role, "Admin")
                }, "Cookies");

                return new ClaimsPrincipal(identity);
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}
