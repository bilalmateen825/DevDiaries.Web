using DevDiaries.Web.Data.Contracts;
using DevDiaries.Web.Models.Blogs;

namespace DevDiaries.Web.Data
{
    public class TagRepository : ITagRepository
    {
        private readonly BlogsDBContext m_blogsDBContext;

        public TagRepository(BlogsDBContext blogsDBContext)
        {
            this.m_blogsDBContext = blogsDBContext;
        }

        public async Task<List<Tag>> GetAllTagsAsync(string stTagName)
        {
            return m_blogsDBContext.Tag.Where(x => x.Name == stTagName).ToList();
        }
    }
}
