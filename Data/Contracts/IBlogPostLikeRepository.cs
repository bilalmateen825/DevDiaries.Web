using DevDiaries.Web.Models.Blogs;

namespace DevDiaries.Web.Data.Contracts
{
    public interface IBlogPostLikeRepository
    {
        Task<int> GetTotalLikesForBlog(Guid blogPostId);

        Task AddLikeForBlog(Guid blogPostId, Guid userId);

        Task<IEnumerable<BlogPostLike>> GetLikesForBlog(Guid blogPostId);
    }
}
