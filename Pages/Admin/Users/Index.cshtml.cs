using DevDiaries.Web.Data.Contracts;
using DevDiaries.Web.Models.Users;
using DevDiaries.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DevDiaries.Web.Pages.Users
{
    [Authorize(Roles = "SuperAdmin")]
    public class IndexModel : PageModel
    {
        private readonly IUserRepository userRepository;

        public List<User> Users { get; set; }

        [BindProperty]
        public AddUser AddUserRequest { get; set; }

        [BindProperty]
        public Guid SelectedUserId { get; set; }

        public IndexModel(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<IActionResult> OnGet()
        {
            var users = await userRepository.GetAllAsync();

            Users = new List<User>();

            foreach (var user in users)
            {
                Users.Add(new Models.Users.User()
                {
                    Id = Guid.Parse(user.Id),
                    Email = user.Email,
                    Username = user.UserName,
                });
            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            IdentityUser identityUser = new IdentityUser
            {
                Email = AddUserRequest.Email,
                UserName = AddUserRequest.Username,
            };

            List<string> lstRoles = new List<string>()
            {
                "User"
            };

            if (AddUserRequest.AdminCheckbox)
                lstRoles.Add("Admin");

            var result = await userRepository.AddAsync(identityUser, AddUserRequest.Password, lstRoles);

            if (result)
                return RedirectToPage("/Admin/Users/Index");

            return Page();
        }

        public async Task<IActionResult> OnPostDelete()
        {
            await userRepository.DeleteAsync(SelectedUserId);
            return RedirectToPage("/Admin/Users/Index");
        }
    }
}
