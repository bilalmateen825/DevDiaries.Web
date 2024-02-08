using DevDiaries.Web.Data.Contracts;
using DevDiaries.Web.Models.Blogs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DevDiaries.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public readonly IBlogRepository m_blogRepository;
        private readonly ITagRepository m_tagRepository;

        public List<Blog> Blogs { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IBlogRepository blogRepository, ITagRepository tagRepository)
        {
            _logger = logger;
            m_blogRepository = blogRepository;
            this.m_tagRepository = tagRepository;
        }


        public async Task<IActionResult> OnGet()
        {
            Blogs = await m_blogRepository.GetBlogsAsync() as List<Blog>;
            return Page();
        }
    }
}