using AutoMapper;
using Blog.FK.Domain.Entities;
using Blog.FK.Web.ViewModels;
using System;

namespace Blog.FK.Web.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserViewModel>();
            CreateMap<UserViewModel, User>()
                .AfterMap((vm, user) => 
                {
                    if (user.Id.Equals(Guid.Empty))
                        user.Id = Guid.NewGuid();

                    if (user.CreatedAt.Equals(DateTime.MinValue))
                        user.CreatedAt = DateTime.Now;

                    if (user.UpdatedAt.Equals(DateTime.MinValue))
                        user.UpdatedAt = DateTime.Now;
                });
        }
    }
}
