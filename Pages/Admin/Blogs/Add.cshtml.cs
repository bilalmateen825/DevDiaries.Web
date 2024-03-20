using DevDiaries.Web.Classes;
using DevDiaries.Web.Data;
using DevDiaries.Web.Data.Contracts;
using DevDiaries.Web.Models.Blogs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.SqlServer.Server;
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
        public Blog BlogPost { get; set; }

        [BindProperty]
        public IFormFile FeaturedImage { get; set; }

        [BindProperty]
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
                Visible = BlogPost.Visible,
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

        public IActionResult OnUpload(IFormFile formFile)
        {
            return Page();
        }
    }
}
