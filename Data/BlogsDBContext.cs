using DevDiaries.Web.Models.Blogs;
using Microsoft.EntityFrameworkCore;

namespace DevDiaries.Web.Data
{
    public class BlogsDBContext : DbContext
    {
        public BlogsDBContext(DbContextOptions<BlogsDBContext> options) : base(options)
        {
        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogPostLike> BlogPostLike { get; set; }
        public DbSet<Tag> Tag { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        //    modelBuilder.Entity<Blog>()
        //.HasMany(e => e.Likes)
        //.WithOne(e => e.BlogPostId)
        //.HasForeignKey(e => e.BlogId)
        //.IsRequired(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}
