using Blog.FK.Application.Interfaces;
using Blog.FK.Infra.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lib.Net.Http.WebPush;
using Model = Blog.FK.Domain.Entities;

namespace Blog.FK.Application
{
    public class PushSubscriptionApplication : IPushSubscriptionApplication, IDisposable
    {
        #region "  Properties  "

        private readonly PushSubscriptionContext _context;

        #endregion

        #region "  Constructor  "

        public PushSubscriptionApplication(PushSubscriptionContext pushSubscription)
        {
            _context = pushSubscription;
        }

        #endregion

        #region "  IPushSubscriptionApplication  "

        public Task DiscardSubscriptionAsync(string endpoint)
        {
            throw new NotImplementedException();
        }        

        public Task ForEachSubscriptionAsync(Action<PushSubscription> action)
        {
            throw new NotImplementedException();
        }

        public Task ForEachSubscriptionAsync(Action<PushSubscription> action, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<int> StoreSubscriptionAsync(PushSubscription subscription)
        {
            if (_context.Subscriptions.Any(s => s.Endpoint == subscription.Endpoint))
                return await Task.FromResult(0);

            await _context.Subscriptions.AddAsync(new Model.PushSubscription(subscription));

            return await _context.SaveChangesAsync();
        }

        #endregion

        #region "  IDisposable  "

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
