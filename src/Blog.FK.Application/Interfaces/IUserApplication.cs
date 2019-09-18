using Blog.FK.Domain.Entities;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blog.FK.Application.Interfaces
{
    public interface IUserApplication : IBaseApplication<User>
    {
        /// <summary>
        /// Authenticate user if exists on database
        /// </summary>
        /// <param name="user">User to authenticate</param>7
        /// <returns>The user Claims</returns>
        Task<ClaimsPrincipal> AuthenticateAsync(User user);
    }
}
