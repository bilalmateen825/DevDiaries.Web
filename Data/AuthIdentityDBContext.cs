using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DevDiaries.Web.Data
{
    public class AuthIdentityDBContext : IdentityDbContext
    {
        public AuthIdentityDBContext(DbContextOptions<AuthIdentityDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            string stUserRoleId = "9acc1eef-a10e-46d9-a8c9-9d48134f8bb0";
            string stAdminRoleId = "6dc1ad5c-3044-43fe-a024-fd3db3ab67b0";
            string stSuperAdminRoleId = "df13a21d-48db-4b6c-9b98-c6ec507bf6f8";

            //Creating Roles for User, Admin, SuperAdmin
            List<IdentityRole> lstRoles = new List<IdentityRole>()
            {
                new IdentityRole()
                {
                    Id = stUserRoleId,
                    Name="User",
                    NormalizedName="User",
                    ConcurrencyStamp=stUserRoleId,
                },
                new IdentityRole()
                {
                    Id=stAdminRoleId,
                    Name="Admin",
                    NormalizedName="Admin",
                    ConcurrencyStamp=stAdminRoleId,
                },
                new IdentityRole()
                {
                    Id=stSuperAdminRoleId,
                    Name="SuperAdmin",
                    NormalizedName="SuperAdmin",
                    ConcurrencyStamp=stSuperAdminRoleId,
                }
            };

            builder.Entity<IdentityRole>().HasData(lstRoles);

            //Seed (user, admin, superAdmin)

            string stSuperAdminUserId = "57cea58d-5a7a-42f3-bec9-7f822326ba6e";
            IdentityUser user = new IdentityUser()
            {
                Id = stSuperAdminUserId,
                Email = "superadmin@devdiaries.com",
                UserName = "superadmin@devdiaries.com",
                NormalizedEmail = "superadmin@devdiaries.com".ToUpper(),
                NormalizedUserName = "superadmin@devdiaries.com".ToUpper()
            };

            user.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(user, "superadmin123");

            builder.Entity<IdentityUser>().HasData(user);

            //Assigning all roles to Super Admin User

            List<IdentityUserRole<string>> lstSuperAdminIdentityUserRoles = new List<IdentityUserRole<string>>()
            {
                new IdentityUserRole<string>
                {
                    RoleId =stSuperAdminRoleId,
                    UserId=stSuperAdminUserId ,
                },
                new IdentityUserRole<string>
                {
                    RoleId =stAdminRoleId ,
                    UserId=stSuperAdminUserId,
                },
                new IdentityUserRole<string>
                {
                    RoleId =stUserRoleId,
                    UserId=stSuperAdminUserId,
                },
            };

            builder.Entity<IdentityUserRole<string>>().HasData(lstSuperAdminIdentityUserRoles);
        }
    }
}
