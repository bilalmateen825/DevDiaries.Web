using DevDiaries.Web.Classes;
using DevDiaries.Web.Data;
using DevDiaries.Web.Data.Contracts;
using DevDiaries.Web.Models.Blogs;
using DevDiaries.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.SqlServer.Server;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text.Json;

namespace DevDiaries.Web.Pages.Admin.Blogs
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class AddModel : PageModel
    {
        private IWebHostEnvironment _hostingEnvironment;

        private readonly IBlogRepository blogRepository;

        [BindProperty]
        public AddBlogPostRequest BlogPost { get; set; }

        [BindProperty]
        public IFormFile FeaturedImage { get; set; }

        [BindProperty]
        [Required]
        public string Tags { get; set; }

        public AddModel(IBlogRepository blogRepository, IWebHostEnvironment webHostEnvironment)
        {
            _hostingEnvironment = webHostEnvironment;
            this.blogRepository = blogRepository;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            string uploadPath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");

            string uploadedFileURL = await Utility.UploadImage(uploadPath, FeaturedImage);

            if (string.IsNullOrEmpty(uploadedFileURL))
                return Page();

            ValidateBlogPost();

            if (ModelState.IsValid)
            {
                Blog blog = new Blog()
                {
                    Author = BlogPost.Author,
                    Content = BlogPost.Content,
                    FeaturedImageURL = uploadedFileURL,// BlogPost.FeaturedImageURL,
                    Heading = BlogPost.Heading,
                    PageTitle = BlogPost.PageTitle,
                    PublishedDate = BlogPost.PublishedDate,
                    ShortDescription = BlogPost.ShortDescription,
                    UrlHandle = BlogPost.UrlHandle,
                    Visible = BlogPost.IsVisible,
                    Tags = DataUtility.ParseTags(Tags),
                };

                await blogRepository.AddBlogAsync(blog);

                Notification notification = new Notification()
                {
                    Message = "Blog added successfully.",
                    Type = Classes.Enums.ENNotificationType.Success
                };

                TempData[Constants.Notification] = JsonSerializer.Serialize<Notification>(notification);

                return RedirectToPage("/Admin/Blogs/Blogs");
            }

            return Page();
        }

        private void ValidateBlogPost()
        {
            if(BlogPost.PublishedDate.Date < DateTime.Now.Date)
            {
                ModelState.AddModelError("BlogPost.PublishedDate",
                    $"{nameof(BlogPost.PublishedDate)} can only be today's date or future date");
            }
        }

        public IActionResult OnUpload(IFormFile formFile)
        {
            return Page();
        }
    }
}
