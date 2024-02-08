namespace DevDiaries.Web.Classes
{
    public class Utility
    {
        public static async Task<string> UploadImage(string stURL, IFormFile file)
        {
            //string uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");

            // Ensure the uploads directory exists
            if (!Directory.Exists(stURL))
            {
                Directory.CreateDirectory(stURL);
            }

            string filePath = Path.Combine(stURL, Guid.NewGuid().ToString() + "_" + file.FileName);

            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return filePath;
        }

    }
}
