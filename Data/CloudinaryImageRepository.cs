using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DevDiaries.Web.Data.Contracts;

namespace DevDiaries.Web.Data
{
    public class CloudinaryImageRepository : IImageRepository
    {
        private Account account = null;


        public CloudinaryImageRepository(IConfiguration configuration)
        {
            account = new Account(
          configuration.GetSection("Cloudinary")["CloudName"],
          configuration.GetSection("Cloudinary")["ApiKey"],
          configuration.GetSection("Cloudinary")["ApiSecret"]);
        }

        public async Task<string> UploadAsync(IFormFile file)
        {
            Cloudinary cloudinary = new Cloudinary(account);

            //var uploadParams = new ImageUploadParams()
            //{
            //    File = new FileDescription(file.Name, file.OpenReadStream()),
            //    DisplayName = file.Name
            //};

            var uploadFileResult = await cloudinary.UploadAsync(
                new ImageUploadParams()
                {
                    File = new FileDescription(file.Name, file.OpenReadStream()),
                    DisplayName = file.Name
                });

            //var uploadResult = await cloudinary.UploadAsync(uploadParams);

            //if (uploadResult != null && uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            //{
            //    return uploadResult.SecureUri.ToString();
            //}

            if (uploadFileResult != null && uploadFileResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return uploadFileResult.SecureUri.ToString();
            }

            return null;
        }
    }
}
