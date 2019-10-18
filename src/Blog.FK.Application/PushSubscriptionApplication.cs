using Blog.FK.Application.Interfaces;
using Blog.FK.Infra.DataContext;
using Lib.Net.Http.WebPush;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

        public async Task DiscardSubscriptionAsync(string endpoint)
        {
            var subscription = await _context.Subscriptions.FindAsync(endpoint);

            if (subscription != null)
            {
                _context.Subscriptions.Remove(subscription);
            }

            await _context.SaveChangesAsync();
        }

        public async Task ForEachSubscriptionAsync(Action<PushSubscription> action)
        {
            await ForEachSubscriptionAsync(action, CancellationToken.None);
        }

        public async Task ForEachSubscriptionAsync(Action<PushSubscription> action, CancellationToken cancellationToken)
        {
            await _context.Subscriptions.AsNoTracking().ForEachAsync(action, cancellationToken);
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
