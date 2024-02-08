using DevDiaries.Web.Classes;
using DevDiaries.Web.Data;
using DevDiaries.Web.Data.Contracts;
using DevDiaries.Web.Models.Blogs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text.Json;

namespace DevDiaries.Web.Pages.Admin.Blogs
{
    public class EditModel : PageModel
    {
        private readonly IBlogRepository m_blogRepository;
        private readonly IWebHostEnvironment m_webHostEnvironment;
        private readonly IImageRepository m_imageRepository;

        private readonly IHttpClientFactory m_httpClientFactory;

        [BindProperty]
        public IFormFile FeaturedImage
        {
            get; set;
        }

        [BindProperty]
        public Blog BlogPost { get; set; }

        [BindProperty]
        public string Tags { get; set; }

        public EditModel(IBlogRepository blogRepository, IWebHostEnvironment webHostEnvironment, IImageRepository imageRepository, IHttpClientFactory httpClientFactory)
        {
            m_blogRepository = blogRepository;
            m_webHostEnvironment = webHostEnvironment;
            m_imageRepository = imageRepository;
            m_httpClientFactory = httpClientFactory;
        }

        public async Task OnGet(Guid id)
        {
            var blog = await m_blogRepository.GetBlogAsync(id);

            if (blog == null)
                return;

            Tags = DataUtility.GetConcatenatedTags(blog.Tags);

            BlogPost = blog;

            //SetFeaturedImageFromUrl(BlogPost.FeaturedImageURL);
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            //string uploadPath = Path.Combine(m_webHostEnvironment.WebRootPath, "uploads");

            //string uploadedFileURL = await Utility.UploadImage(uploadPath, FeaturedImage);
            string uploadedFileURL = await m_imageRepository.UploadAsync(FeaturedImage);

            BlogPost.Tags = DataUtility.ParseTags(Tags);

            if (!string.IsNullOrEmpty(uploadedFileURL))
                BlogPost.FeaturedImageURL = uploadedFileURL;

            BlogPost.Tags = DataUtility.ParseTags(Tags);

            await m_blogRepository.UpdateBlogAsync(BlogPost);

            Notification notification = new Notification()
            {
                Message = "Blog updated successfully.",
                Type = Classes.Enums.ENNotificationType.Success
            };

            TempData[Constants.Notification] = JsonSerializer.Serialize<Notification>(notification);

            return RedirectToPage("/Admin/Blogs/Blogs");
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            await m_blogRepository.DeleteBlogAsync(BlogPost.Id);

            Notification notification = new Notification()
            {
                Message = "Blog deleted successfully.",
                Type = Classes.Enums.ENNotificationType.Success
            };

            TempData[Constants.Notification] = JsonSerializer.Serialize<Notification>(notification);

            return RedirectToPage("/Admin/Blogs/Blogs");
        }

        public async Task SetFeaturedImageFromUrl(string imageUrl)
        {
            using (var httpClient = m_httpClientFactory.CreateClient())
            {
                var response = await httpClient.GetAsync(imageUrl);

                if (response.IsSuccessStatusCode)
                {
                    var fileName = Path.GetFileName(imageUrl);
                    var localFilePath = Path.Combine(m_webHostEnvironment.WebRootPath, $"uploads\\{fileName}");

                    using (var fileStream = new FileStream(localFilePath, FileMode.Create))
                    {
                        await response.Content.CopyToAsync(fileStream);
                    }

                    FeaturedImage = new FormFile(new FileStream(localFilePath, FileMode.Open), 0, new FileInfo(localFilePath).Length, "FeaturedImage", fileName);
                }
                else
                {
                    // Handle error if necessary
                }
            }
        }
    }
}
