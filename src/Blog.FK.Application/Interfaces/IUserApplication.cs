using Blog.FK.Domain.Entities;
using System.Threading.Tasks;

namespace Blog.FK.Application.Interfaces
{
    public interface IUserApplication : IBaseApplication<User>
    {
        /// <summary>
        /// Authenticate user if exists on database
        /// </summary>
        /// <param name="user">User to authenticate</param>
        /// <returns>A JWT token</returns>
        Task<Token> AuthenticateAsync(User user);
    }
}
