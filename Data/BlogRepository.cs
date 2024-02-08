using DevDiaries.Web.Classes;
using DevDiaries.Web.Data.Contracts;
using DevDiaries.Web.Models.Blogs;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace DevDiaries.Web.Data
{
    public class BlogRepository : IBlogRepository
    {
        private readonly BlogsDBContext blogsDBContext;

        public BlogRepository(BlogsDBContext blogsDBContext)
        {
            this.blogsDBContext = blogsDBContext;
        }

        public async Task<Blog> AddBlogAsync(Blog blog)
        {
            if (blogsDBContext == null || blog == null)
                return null;

            await blogsDBContext.Blogs.AddAsync(blog);
            await blogsDBContext.SaveChangesAsync();
            return blog;
        }

        public async Task<bool> DeleteBlogAsync(Guid id)
        {
            if (blogsDBContext == null)
                return false;

            Blog blogToRemove = await blogsDBContext.Blogs.FirstOrDefaultAsync(x => x.Id == id);

            if (blogToRemove == null)
                return false;

            blogsDBContext.Blogs.Remove(blogToRemove);
            await blogsDBContext.SaveChangesAsync();
            return true;
        }

        public async Task<Blog> GetBlogAsync(Guid id)
        {
            if (blogsDBContext == null)
                return null;

            Blog blog = await blogsDBContext.Blogs.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == id);

            return blog;
        }

        public async Task<Blog> GetBlogAsync(string stUrlHandle)
        {
            if (blogsDBContext == null)
                return null;

            Blog blog = await blogsDBContext.Blogs.Include(x => x.Tags).FirstOrDefaultAsync(x => x.UrlHandle == stUrlHandle);

            return blog;
        }

        public async Task<IEnumerable<Blog>> GetBlogsAsync(string stTagName)
        {
            if (blogsDBContext == null)
                return null;

            return await (blogsDBContext.Blogs.Include(x=>x.Tags).Where(x => x.Tags.Any(x => x.Name == stTagName))).ToListAsync();
        }

        public async Task<IEnumerable<Blog>> GetBlogsAsync()
        {
            if (blogsDBContext == null)
                return null;

            return await blogsDBContext.Blogs.Include(x=>x.Tags).ToListAsync();
        }

        public async Task<Blog> UpdateBlogAsync(Blog blog)
        {
            if (blog == null)
                return null;

            Blog blogToUpdate = await blogsDBContext.Blogs.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == blog.Id);
           
            if (blogToUpdate == null)
                return null;

            blogToUpdate.Author = blog.Author;
            blogToUpdate.ShortDescription = blog.ShortDescription;
            blogToUpdate.FeaturedImageURL = blog.FeaturedImageURL;
            blogToUpdate.Content = blog.Content;
            blogToUpdate.PageTitle = blog.PageTitle;
            blogToUpdate.Heading = blog.Heading;
            blogToUpdate.UrlHandle = blog.UrlHandle;
            blogToUpdate.Visible = blog.Visible;
            blogToUpdate.PublishedDate = blog.PublishedDate;

            if(DataUtility.TagChanged(blogToUpdate.Tags,blog.Tags))
            {
                blogToUpdate.Tags = blog.Tags;
            }

            await blogsDBContext.SaveChangesAsync();
            return blog;
        }
    }
}
