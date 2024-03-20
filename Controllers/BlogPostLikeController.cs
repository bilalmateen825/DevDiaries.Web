using DevDiaries.Web.Data.Contracts;
using DevDiaries.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DevDiaries.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogPostLikeController : Controller
    {
        private readonly IBlogPostLikeRepository m_blogPostLikeRepository;

        public BlogPostLikeController(IBlogPostLikeRepository blogPostLikeRepository)
        {
            this.m_blogPostLikeRepository = blogPostLikeRepository;
        }

        [Route("Add")]
        [HttpPost]
        public async Task<IActionResult> AddLike([FromBody] AddBlogPostLikeRequest addBlogPostLikeRequest)
        {
            await m_blogPostLikeRepository.AddLikeForBlog(addBlogPostLikeRequest.BlogPostId, addBlogPostLikeRequest.UserId);

            return Ok();
        }

        [HttpGet]
        [Route("{blogPostId:Guid}/totalLikes")]
        public async Task<IActionResult> GetTotalLikes([FromRoute] Guid blogPostId)
        {
            var totalLikes = await m_blogPostLikeRepository.GetTotalLikesForBlog(blogPostId);
            return Ok(totalLikes);
        }
    }
}
