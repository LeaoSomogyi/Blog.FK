using AutoMapper;
using Blog.FK.Web.ViewModels;
using Lib.Net.Http.WebPush;

namespace Blog.FK.Web.Profiles
{
    public class PushMessageProfile : Profile
    {
        public PushMessageProfile()
        {
            CreateMap<PushMessage, PushMessageViewModel>();
            CreateMap<PushMessageViewModel, PushMessage>()
                .ForMember(mo => mo.HttpContent, mo => mo.Ignore())
                .ForMember(mo => mo.TimeToLive, mo => mo.Ignore());
        }
    }
}
