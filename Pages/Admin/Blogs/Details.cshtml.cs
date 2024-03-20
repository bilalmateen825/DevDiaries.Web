using DevDiaries.Web.Data.Contracts;
using DevDiaries.Web.Models.Blogs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DevDiaries.Web.Pages.Admin.Blogs
{
    public class DetailsModel : PageModel
    {
        private readonly IBlogRepository blogRepository;
        private readonly IBlogPostLikeRepository blogPostLikeRepository;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;

        public Blog Blog { get; set; }
        public int Likes { get; set; }

        public bool Liked { get; set; }

        public DetailsModel(IBlogRepository blogRepository, IBlogPostLikeRepository blogPostLikeRepository, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            this.blogRepository = blogRepository;
            this.blogPostLikeRepository = blogPostLikeRepository;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        public async Task<IActionResult> OnGet(string urlHandle)
        {
            Blog = await blogRepository.GetBlogAsync(urlHandle);

            if(Blog!= null)
            {
                if(signInManager.IsSignedIn(User))
                {
                    var likes = await blogPostLikeRepository.GetLikesForBlog(Blog.Id);

                    var user = userManager.GetUserId(User);

                    Liked = likes.Any(x => x.UserId == Guid.Parse(user));
                }
            }

            Likes = await blogPostLikeRepository.GetTotalLikesForBlog(Blog.Id);
            return Page();
        }
    }
}
