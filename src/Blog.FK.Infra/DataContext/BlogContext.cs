using Blog.FK.Domain.Entities;
using Blog.FK.Infra.EFConfig;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Blog.FK.Infra.DataContext
{
    public class BlogContext : DbContext
    {
        public virtual DbSet<BlogPost> BlogPosts { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public BlogContext(DbContextOptions<BlogContext> dbContextOptions) : base(dbContextOptions) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BlogPostEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
