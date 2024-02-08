namespace DevDiaries.Web.HTTPHelper
{
    public class HTTPHelper
    {
        public static async Task<bool> UploadFile(string stURL, IFormFile file)
        {
            using (var httpClient = new HttpClient())
            {
                using (var formData = new MultipartFormDataContent())
                {
                    using (var fileStream = file.OpenReadStream())
                    {
                        formData.Add(new StreamContent(fileStream), "FeaturedImage", file.FileName);
                        var response = await httpClient.PostAsync(stURL, formData);

                        if(response.IsSuccessStatusCode)
                            return true;

                        return false;
                    }
                }
            }         
        }
    }
}
