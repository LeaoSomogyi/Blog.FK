using System.Collections.Generic;

namespace Blog.FK.Web.ViewModels
{
    public class PushSubscriptionViewModel
    {
        public string Endpoint { get; set; }

        public IDictionary<string, string> Keys { get; set; }

        public PushSubscriptionViewModel() { }
    }
}
