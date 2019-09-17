using Blog.FK.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.FK.Infra.EFConfig
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> userConfiguration)
        {
            userConfiguration.ToTable("TFKUser");

            userConfiguration.HasKey(u => u.Id);

            userConfiguration.Property(u => u.Name)
                .HasColumnType("NVARCHAR(256)")
                .HasColumnName("Name_User")
                .IsRequired(true);

            userConfiguration.Property(u => u.Email)
                .HasColumnType("NVARCHAR(256)")
                .HasColumnName("Email_User")
                .IsRequired(true);

            userConfiguration.Property(u => u.Password)
                .HasColumnType("NVARCHAR(MAX)")
                .HasColumnName("Password_User")
                .IsRequired(true);

            userConfiguration.Property(u => u.CreatedAt)
                .HasColumnType("Datetime")
                .HasColumnName("CreatedAt_User")
                .IsRequired(true);

            userConfiguration.Property(u => u.UpdatedAt)
                .HasColumnType("Datetime")
                .HasColumnName("UpdatedAt_User")
                .IsRequired(true);
        }
    }
}
