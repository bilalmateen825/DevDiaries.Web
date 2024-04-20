using DevDiaries.Web.Data.Contracts;
using DevDiaries.Web.Models.Blogs;
using Microsoft.EntityFrameworkCore;

namespace DevDiaries.Web.Data
{
    public class BlogPostCommentRepository : IBlogPostCommentRepository
    {
        private readonly BlogsDBContext m_blogsDBContext;

        public BlogPostCommentRepository(BlogsDBContext blogsDBContext)
        {
            this.m_blogsDBContext = blogsDBContext;
        }

        public async Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment)
        {
            await m_blogsDBContext.BlogPostComment.AddAsync(blogPostComment);
            await m_blogsDBContext.SaveChangesAsync();
            return blogPostComment;
        }

        public async Task<IEnumerable<BlogPostComment>> GetAllAsync(Guid blogPostId)
        {
            return await m_blogsDBContext.BlogPostComment.Where(x => x.BlogPostId == blogPostId).ToListAsync();
        }
    }
}
