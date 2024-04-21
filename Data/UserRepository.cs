using DevDiaries.Web.Data.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DevDiaries.Web.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthIdentityDBContext authIdentityDBContext;
        private readonly UserManager<IdentityUser> userManager;

        public UserRepository(AuthIdentityDBContext authIdentityDBContext, UserManager<IdentityUser> userManager)
        {
            this.authIdentityDBContext = authIdentityDBContext;
            this.userManager = userManager;
        }

        public async Task<bool> AddAsync(IdentityUser identityUser, string password, List<string> roles)
        {
            var identityResult = await userManager.CreateAsync(identityUser, password);

            if (identityResult.Succeeded)
            {
                identityResult = await userManager.AddToRolesAsync(identityUser, roles);

                if (identityResult.Succeeded)
                    return true;
            }

            return false;
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<IdentityUser>> GetAllAsync()
        {
            return (IEnumerable<IdentityUser>)await authIdentityDBContext.Users.Where(x => x.Email != "superadmin@devdiaries.com").ToListAsync();
        }
    }
}
