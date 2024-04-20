using DevDiaries.Web.Models.Blogs;

namespace DevDiaries.Web.Data.Contracts
{
    public interface IBlogPostCommentRepository
    {
        Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment);
        Task<IEnumerable<BlogPostComment>> GetAllAsync(Guid blogPostId);
    }
}
