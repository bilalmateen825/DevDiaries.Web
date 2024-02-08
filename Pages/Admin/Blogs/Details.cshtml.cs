using DevDiaries.Web.Data.Contracts;
using DevDiaries.Web.Models.Blogs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DevDiaries.Web.Pages.Admin.Blogs
{
    public class DetailsModel : PageModel
    {
        private readonly IBlogRepository blogRepository;

        public Blog Blog { get; set; }

        public DetailsModel(IBlogRepository blogRepository)
        {
            this.blogRepository = blogRepository;
        }

        public async Task<IActionResult> OnGet(string urlHandle)
        {
            Blog = await blogRepository.GetBlogAsync(urlHandle);
            return Page();
        }
    }
}
