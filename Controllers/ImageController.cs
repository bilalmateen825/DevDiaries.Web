using DevDiaries.Web.Data.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
using System;
using System.Net;

namespace DevDiaries.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageController : Controller
    {
        private readonly IImageRepository m_imageRepository;

        public ImageController(IImageRepository imageRepository)
        {
            m_imageRepository = imageRepository;
            //_hostingEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok("This is the Images Get method!");
        }

        [HttpPost]
        [Route("UploadAsync")]
        public async Task<IActionResult> UploadAsync(IFormFile file)
        {
            string stUploadImageURL = await m_imageRepository.UploadAsync(file);

            if (string.IsNullOrEmpty(stUploadImageURL))
                return Problem("Something went wrong!", null, (int)HttpStatusCode.InternalServerError);

            //byte[] imageBytes = await System.IO.File.ReadAllBytesAsync(stUploadImageURL);

            //// Create a memory stream from the image bytes
            //using (MemoryStream memoryStream = new MemoryStream(imageBytes))
            //{
            //    // Create a Blob URL
            //    string blobUrl = CreateBlobUrl(memoryStream);
            //    return Json(blobUrl);
            //    // Use the Blob URL as needed
            //    Console.WriteLine($"Blob URL: {blobUrl}");
            //}

            //return Json("blob:http://localhost:5115/uploads/cat.jpg");
            return Json(new { link = stUploadImageURL });
        }

        static string CreateBlobUrl(MemoryStream memoryStream)
        {
            ByteArrayContent content = new ByteArrayContent(memoryStream.ToArray());

            // Convert the byte array to a Base64 string
            string base64String = Convert.ToBase64String(memoryStream.ToArray());

            // Create the Blob URL
            string blobUrl = $"data:image/jpeg;base64,{base64String}";

            return blobUrl;
        }
    }
}
