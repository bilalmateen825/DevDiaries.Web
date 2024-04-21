using Microsoft.AspNetCore.Identity;

namespace DevDiaries.Web.Data.Contracts
{
    public interface IUserRepository
    {
        Task<IEnumerable<IdentityUser>> GetAllAsync();
        Task<bool> AddAsync(IdentityUser identityUser, string password, List<string> roles);
        Task<bool> DeleteAsync(int id);
    }
}
