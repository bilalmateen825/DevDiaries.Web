using DevDiaries.Web.Models.Blogs;

namespace DevDiaries.Web.Data.Contracts
{
    public interface ITagRepository
    {
        public Task<List<Tag>> GetAllTagsAsync(string stTagName);
    }
}
