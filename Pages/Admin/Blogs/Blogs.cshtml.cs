using DevDiaries.Web.Classes;
using DevDiaries.Web.Data;
using DevDiaries.Web.Data.Contracts;
using DevDiaries.Web.Models.Blogs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace DevDiaries.Web.Pages.Admin.Blogs
{
    public class BlogsModel : PageModel
    {
        private readonly BlogsDBContext m_dbContext;
        private readonly IBlogRepository blogRepository;

        [BindProperty]
        public List<Blog> Blogs { get; set; }

        public BlogsModel(IBlogRepository blogRepository)
        {
            this.blogRepository = blogRepository;
        }

        public async Task OnGet()
        {
            if (blogRepository == null)
                return;

            var notification = TempData[Constants.Notification];

            if(notification != null)
            {
                ViewData[Constants.Notification] = JsonSerializer.Deserialize<Notification>(notification.ToString());
            }

            Blogs = (await blogRepository.GetBlogsAsync())?.ToList();
        }
    }
}
