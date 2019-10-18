using Lib.Net.Http.WebPush;

namespace Blog.FK.Web.ViewModels
{
    public class PushMessageViewModel
    {
        public string Topic { get; set; }

        public string Content { get; set; }

        public PushMessageUrgency Urgency { get; set; }

        public PushMessageViewModel()
        {
            Urgency = PushMessageUrgency.Normal;
        }
    }
}
