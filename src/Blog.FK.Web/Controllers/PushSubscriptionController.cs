using AutoMapper;
using Blog.FK.Application.Interfaces;
using Blog.FK.Web.ViewModels;
using Lib.Net.Http.WebPush;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Blog.FK.Web.Controllers
{
    [Route("subscriptions")]
    [ApiController]
    public class PushSubscriptionController : ControllerBase
    {
        #region "  Applications & Services  "

        private readonly IMapper _mapper;
        private readonly IPushSubscriptionApplication _pushSubscriptionApp;
        private readonly PushServiceClient _pushServiceClient;

        #endregion

        #region "  Constructors  "

        public PushSubscriptionController(IMapper mapper,
            IPushSubscriptionApplication pushSubscriptionApp,
            PushServiceClient pushServiceClient)
        {
            _mapper = mapper;
            _pushSubscriptionApp = pushSubscriptionApp;
            _pushServiceClient = pushServiceClient;
        }

        #endregion

        #region "  Methods  "

        [HttpGet]
        [Route("public-key")]
        public async Task<IActionResult> GetPublicKey()
        {
            return await Task.Run(() =>
            {
                return Content(_pushServiceClient.DefaultAuthentication.PublicKey, "text/plain");
            });
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> StoreSubscription(PushSubscriptionViewModel pushSubscriptionViewModel)
        {
            var pushSubscription = _mapper.Map<PushSubscription>(pushSubscriptionViewModel);

            var response = await _pushSubscriptionApp.StoreSubscriptionAsync(pushSubscription);

            if (response > 0)
                return CreatedAtAction(nameof(StoreSubscription), pushSubscription);

            return NoContent();
        }

        [HttpDelete]
        [Route("{endpoint}")]
        public async Task<IActionResult> DiscardSubscription(string endpoint)
        {
            await _pushSubscriptionApp.DiscardSubscriptionAsync(endpoint);

            return NoContent();
        }

        [HttpPost]
        [Route("send-notification")]
        public async Task<IActionResult> SendNotification(PushMessageViewModel pushMessageViewModel)
        {
            var message = _mapper.Map<PushMessage>(pushMessageViewModel);

            await _pushSubscriptionApp.ForEachSubscriptionAsync((subscription) =>
            {
                _pushServiceClient.RequestPushMessageDeliveryAsync(subscription, message);
            });

            return NoContent();
        }

        #endregion
    }
}