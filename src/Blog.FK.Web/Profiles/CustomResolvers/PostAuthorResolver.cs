using AutoMapper;
using Blog.FK.Domain.Entities;
using Blog.FK.Web.ViewModels;

namespace Blog.FK.Web.Profiles.CustomResolvers
{
    public class PostAuthorResolver : IValueResolver<BlogPost, BlogPostViewModel, UserViewModel>
    {
        public UserViewModel Resolve(BlogPost source, BlogPostViewModel destination, UserViewModel destMember, ResolutionContext context)
        {
            return new UserViewModel()
            {
                Id = source.User.Id,
                Name = source.User.Name,
                Email = source.User.Email
            };
        }
    }
}
