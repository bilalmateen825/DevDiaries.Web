using DevDiaries.Web.Data.Contracts;

namespace DevDiaries.Web.Models.Blogs
{
    public class BlogPostLike //:IBlogPostLike
    {
        public Guid Id { get; set; }
        public Guid BlogPostId { get ; set; }
        public Guid UserId { get; set ; }

        //public Blog Blog { get; set; }
    }
}
