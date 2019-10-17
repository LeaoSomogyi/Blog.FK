using Blog.FK.Domain.Entities;
using Blog.FK.Infra.EFConfig;
using Microsoft.EntityFrameworkCore;

namespace Blog.FK.Infra.DataContext
{
    public class PushSubscriptionContext : DbContext
    {
        public virtual DbSet<PushSubscription> Subscriptions { get; set; }

        public PushSubscriptionContext(DbContextOptions<PushSubscriptionContext> dbContextOptions)
            : base(dbContextOptions) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PushSubscriptionEntityTypeConfiguration());
        }
    }
}
