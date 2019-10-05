using Blog.FK.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.FK.Infra.EFConfig
{
    public class BlogPostEntityTypeConfiguration : IEntityTypeConfiguration<BlogPost>
    {
        public void Configure(EntityTypeBuilder<BlogPost> blogConfiguration)
        {
            blogConfiguration.ToTable("TFKBlogPost");

            blogConfiguration.HasKey(b => b.Id);

            blogConfiguration.Property(b => b.Title)
                .HasColumnType("NVARCHAR(256)")
                .HasColumnName("Title_BlogPost")
                .IsRequired(true);

            blogConfiguration.Property(b => b.ShortDescription)
                .HasColumnType("NVARCHAR(MAX)")
                .HasColumnName("ShortDescription_BlogPost")
                .IsRequired(true);

            blogConfiguration.Property(b => b.CreatedAt)
                .HasColumnType("Datetime")
                .HasColumnName("CreatedAt_BlogPost")
                .IsRequired(true);

            blogConfiguration.Property(b => b.UpdatedAt)
                .HasColumnType("Datetime")
                .HasColumnName("UpdatedAt_BlogPost")
                .IsRequired(true);

            blogConfiguration.HasOne(b => b.User)
                .WithMany(u => u.BlogPosts)
                .HasForeignKey(b => b.UserId);
        }
    }
}
