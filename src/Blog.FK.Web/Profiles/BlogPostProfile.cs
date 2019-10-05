using AutoMapper;
using Blog.FK.Domain.Entities;
using Blog.FK.Web.Profiles.CustomResolvers;
using Blog.FK.Web.ViewModels;
using System;

namespace Blog.FK.Web.Profiles
{
    public class BlogPostProfile : Profile
    {
        public BlogPostProfile()
        {
            CreateMap<BlogPost, BlogPostViewModel>()
                .ForMember(mo => mo.UserViewModel, b => b.MapFrom(new PostAuthorResolver()));

            CreateMap<BlogPostViewModel, BlogPost>()
                .AfterMap((vm, post) =>
                {
                    if (post.Id.Equals(Guid.Empty))
                        post.Id = Guid.NewGuid();

                    if (post.CreatedAt.Equals(DateTime.MinValue))
                        post.CreatedAt = DateTime.Now;

                    if (post.UpdatedAt.Equals(DateTime.MinValue))
                        post.UpdatedAt = DateTime.Now;

                    post.UserId = vm.UserViewModel.Id;
                });
        }
    }
}
