using Blog.FK.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.FK.Infra.EFConfig
{
    public class PushSubscriptionEntityTypeConfiguration : IEntityTypeConfiguration<PushSubscription>
    {
        public void Configure(EntityTypeBuilder<PushSubscription> pushConfiguration)
        {
            pushConfiguration.HasKey(p => p.Endpoint);
            pushConfiguration.Ignore(p => p.Keys);
        }
    }
}
