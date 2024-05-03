using DevDiaries.Web.Classes;
using DevDiaries.Web.Data;
using DevDiaries.Web.Data.Contracts;
using DevDiaries.Web.Models.Blogs;
using DevDiaries.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text.Json;

namespace DevDiaries.Web.Pages.Admin.Blogs
{
    [Authorize(Roles = "Admin,SuperAdmin")]
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
        public EditBlogPostRequest BlogPost { get; set; }

        [BindProperty]
        [Required]
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
            var blogDomainModel = await m_blogRepository.GetBlogAsync(id);

            if (blogDomainModel == null)
                return;

            if (blogDomainModel.Tags != null)
                Tags = DataUtility.GetConcatenatedTags(blogDomainModel.Tags);

            BlogPost = new EditBlogPostRequest()
            {
                Id = blogDomainModel.Id,
                Author = blogDomainModel.Author,
                Content = blogDomainModel.Content,
                FeaturedImageURL = blogDomainModel.FeaturedImageURL,
                Heading = blogDomainModel.Heading,
                PageTitle = blogDomainModel.PageTitle,
                UrlHandle = blogDomainModel.UrlHandle,
                PublishedDate = blogDomainModel.PublishedDate,
                ShortDescription = blogDomainModel.ShortDescription,
                Visible = blogDomainModel.Visible,
            };

            //SetFeaturedImageFromUrl(BlogPost.FeaturedImageURL);
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            //string uploadPath = Path.Combine(m_webHostEnvironment.WebRootPath, "uploads");

            //string uploadedFileURL = await Utility.UploadImage(uploadPath, FeaturedImage);

            if (ModelState.IsValid)
            {
                try
                {
                    Blog blogDomainModel = new Blog()
                    {
                        Id = BlogPost.Id,
                        Author = BlogPost.Author,
                        Content = BlogPost.Content,
                        FeaturedImageURL = BlogPost.FeaturedImageURL,
                        Heading = BlogPost.Heading,
                        PageTitle = BlogPost.PageTitle,
                        UrlHandle = BlogPost.UrlHandle,
                        PublishedDate = BlogPost.PublishedDate,
                        ShortDescription = BlogPost.ShortDescription,
                        Visible = BlogPost.Visible
                    };

                    string uploadedFileURL = await m_imageRepository.UploadAsync(FeaturedImage);

                    blogDomainModel.Tags = DataUtility.ParseTags(Tags);

                    if (!string.IsNullOrEmpty(uploadedFileURL))
                        blogDomainModel.FeaturedImageURL = uploadedFileURL;

                    blogDomainModel.Tags = DataUtility.ParseTags(Tags);

                    await m_blogRepository.UpdateBlogAsync(blogDomainModel);

                    Notification notification = new Notification()
                    {
                        Message = "Blog updated successfully.",
                        Type = Classes.Enums.ENNotificationType.Success
                    };

                    TempData[Constants.Notification] = JsonSerializer.Serialize<Notification>(notification);

                    return RedirectToPage("/Admin/Blogs/Blogs");
                }
                catch (Exception ex)
                {
                    ViewData["Notification"] = new Notification
                    {
                        Type = Classes.Enums.ENNotificationType.Error,
                        Message = "Something went wrong!",
                    };
                }

                return Page();
            }

            return Page();
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

        private void ValidateEditBlogPost()
        {
            if(!string.IsNullOrWhiteSpace(BlogPost.Heading))
            {
                //checking for max length
                if(BlogPost.Heading.Length > 500)
                {
                    //BlogPost.Heading is key
                    ModelState.AddModelError("BlogPost.Heading", "Heading can not be greater than 500 characters");
                }
            }
        }
    }
}
