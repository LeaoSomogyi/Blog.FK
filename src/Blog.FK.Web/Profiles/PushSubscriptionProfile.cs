using AutoMapper;
using Blog.FK.Web.ViewModels;
using Lib.Net.Http.WebPush;

namespace Blog.FK.Web.Profiles
{
    public class PushSubscriptionProfile : Profile
    {
        public PushSubscriptionProfile()
        {
            CreateMap<PushSubscription, PushSubscriptionViewModel>();
            CreateMap<PushSubscriptionViewModel, PushSubscription>();
        }
    }
}
