using DevDiaries.Web.Data.Contracts;
using DevDiaries.Web.Models.Blogs;
using Microsoft.EntityFrameworkCore;

namespace DevDiaries.Web.Data
{
    public class BlogPostLikeRepository : IBlogPostLikeRepository
    {
        private readonly BlogsDBContext m_blogsDBContext;

        public BlogPostLikeRepository(BlogsDBContext blogsDBContext)
        {
            this.m_blogsDBContext = blogsDBContext;
        }

        public async Task AddLikeForBlog(Guid blogPostId, Guid userId)
        {
            BlogPostLike blogPostLike = new BlogPostLike()
            {
                Id = Guid.NewGuid(),
                BlogPostId = blogPostId,
                UserId = userId
            };

            await m_blogsDBContext.BlogPostLike.AddAsync(blogPostLike);
            await m_blogsDBContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<BlogPostLike>> GetLikesForBlog(Guid blogPostId)
        {
            return await m_blogsDBContext.BlogPostLike.Where(x => x.BlogPostId == blogPostId).ToListAsync();
        }

        public async Task<int> GetTotalLikesForBlog(Guid blogPostId)
        {
            var a = m_blogsDBContext.Blogs.Include(x => x.Likes).ToList();
            return await m_blogsDBContext.BlogPostLike.CountAsync(x => x.BlogPostId == blogPostId);
        }
    }
}
