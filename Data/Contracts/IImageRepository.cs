using Microsoft.AspNetCore.Mvc;

namespace DevDiaries.Web.Data.Contracts
{
    public interface IImageRepository
    {
        public Task<string> UploadAsync(IFormFile file);
    }
}
