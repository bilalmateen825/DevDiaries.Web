using DevDiaries.Web.Models.Blogs;

namespace DevDiaries.Web.Data.Contracts
{
    public interface IBlogRepository
    {
        public Task<IEnumerable<Blog>> GetBlogsAsync();
        public Task<IEnumerable<Blog>> GetBlogsAsync(string stTagName);

        public Task<Blog> GetBlogAsync(Guid id);
        public Task<Blog> GetBlogAsync(string stUrlHandle);

        public Task<Blog> AddBlogAsync(Blog blog);
        public Task<Blog> UpdateBlogAsync(Blog blog);
        public Task<bool> DeleteBlogAsync(Guid id);
    }
}
