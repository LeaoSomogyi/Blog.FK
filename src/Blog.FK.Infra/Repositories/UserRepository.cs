using Blog.FK.Domain.Entities;
using Blog.FK.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blog.FK.Infra.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(DbContext dbContext) : base(dbContext) { }
    }
}
