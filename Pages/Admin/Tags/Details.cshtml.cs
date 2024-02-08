using DevDiaries.Web.Data.Contracts;
using DevDiaries.Web.Models.Blogs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DevDiaries.Web.Pages.Admin.Tags
{
    public class DetailsModel : PageModel
    {
        private readonly IBlogRepository m_blogRepository;

        public List<Blog> Blogs { get; set; }

        public DetailsModel(IBlogRepository tagRepository)
        {
            this.m_blogRepository = tagRepository;
        }

        public async Task<IActionResult> OnGet(string tagName)
        {
            tagName = Uri.UnescapeDataString(tagName);

            Blogs = (await m_blogRepository.GetBlogsAsync(tagName)).ToList();
            return Page();
        }
    }
}
