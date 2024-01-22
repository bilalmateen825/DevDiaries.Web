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
        public DbSet<Tag> Tag { get; set; }
    }
}
