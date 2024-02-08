using DevDiaries.Web.Data.Contracts;

namespace DevDiaries.Web.Data
{
    public class ImageRepository : IImageRepository
    {
        private IWebHostEnvironment _hostingEnvironment;
        public ImageRepository(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<string> UploadAsync(IFormFile file)
        {
            if (file == null)
                return null;

            //string uploads =;
            string uploads = "http://localhost:5115/uploads";
            string absolutePath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
            if (file.Length > 0)
            {
                string filePath = Path.Combine(absolutePath, file.FileName);
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                    uploads += $"/{file.FileName}";
                    return uploads;
                }
            }

            return null;
        }
    }
}
