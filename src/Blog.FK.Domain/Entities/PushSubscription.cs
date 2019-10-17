using WebPush = Lib.Net.Http.WebPush;

namespace Blog.FK.Domain.Entities
{
    public class PushSubscription : WebPush.PushSubscription
    {
        public string P256DH
        {
            get { return GetKey(WebPush.PushEncryptionKeyName.P256DH); }

            set { SetKey(WebPush.PushEncryptionKeyName.P256DH, value); }
        }

        public string Auth
        {
            get { return GetKey(WebPush.PushEncryptionKeyName.Auth); }

            set { SetKey(WebPush.PushEncryptionKeyName.Auth, value); }
        }

        public PushSubscription() { }

        public PushSubscription(WebPush.PushSubscription subscription)
        {
            Endpoint = subscription.Endpoint;
            Keys = subscription.Keys;
        }
    }
}
