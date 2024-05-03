using DevDiaries.Web.Data.Contracts;
using DevDiaries.Web.Models.Blogs;
using DevDiaries.Web.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace DevDiaries.Web.Pages.Admin.Blogs
{
    public class DetailsModel : PageModel
    {
        private readonly IBlogRepository blogRepository;
        private readonly IBlogPostLikeRepository blogPostLikeRepository;
        private readonly IBlogPostCommentRepository blogPostCommentRepository;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;

        public Blog Blog { get; set; }
        public int Likes { get; set; }

        public bool Liked { get; set; }

        [BindProperty]
        public Guid BlogPostId { get; set; }

        [BindProperty]
        [Required]
        [MaxLength(500)]
        [MinLength(5)]
        public string CommentDescription { get; set; }

        public IEnumerable<BlogComment> Comments { get; set; }

        public DetailsModel(IBlogRepository blogRepository, IBlogPostLikeRepository blogPostLikeRepository, IBlogPostCommentRepository blogPostCommentRepository, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            this.blogRepository = blogRepository;
            this.blogPostLikeRepository = blogPostLikeRepository;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.blogPostCommentRepository = blogPostCommentRepository;
        }

        public async Task<IActionResult> OnGet(string urlHandle)
        {
            await GetBlog(urlHandle);
            return Page();
        }

        private async Task GetComments()
        {
            var blogComments = await blogPostCommentRepository.GetAllAsync(Blog.Id);
            var blogCommentsViewModel = new List<BlogComment>();


            foreach (var comment in blogComments)
            {
                IdentityUser identityUser = (await userManager.FindByIdAsync(comment.UserId.ToString()));

                if (identityUser == null)
                    continue;

                blogCommentsViewModel.Add(new BlogComment()
                {
                    Description = comment.Description,
                    DateAdded = comment.DateAdded,
                    Username = identityUser.UserName,
                });
            }

            Comments = blogCommentsViewModel;
        }

        private async Task GetBlog(string urlHandle)
        {
            Blog = await blogRepository.GetBlogAsync(urlHandle);

            if (Blog != null)
            {
                BlogPostId = Blog.Id;

                if (signInManager.IsSignedIn(User))
                {
                    var likes = await blogPostLikeRepository.GetLikesForBlog(Blog.Id);

                    var user = userManager.GetUserId(User);

                    Liked = likes.Any(x => x.UserId == Guid.Parse(user));

                    await GetComments();
                }
            }

            Likes = await blogPostLikeRepository.GetTotalLikesForBlog(Blog.Id);
        }

        public async Task<IActionResult> OnPost(string urlHandle)
        {
            if (ModelState.IsValid)
            {
                if (signInManager.IsSignedIn(User) && !string.IsNullOrWhiteSpace(CommentDescription))
                {
                    var userId = userManager.GetUserId(User);

                    var comment = new BlogPostComment()
                    {
                        BlogPostId = BlogPostId,
                        Description = CommentDescription,
                        DateAdded = DateTime.Now,
                        UserId = Guid.Parse(userId),
                    };

                    await blogPostCommentRepository.AddAsync(comment);
                }

                return RedirectToPage("Details", new { urlHandle = urlHandle });
            }

            await GetBlog(urlHandle);
            return Page();
        }
    }
}
