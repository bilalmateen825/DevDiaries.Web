using DevDiaries.Web.Classes;
using DevDiaries.Web.Data;
using DevDiaries.Web.Models.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DevDiaries.Web.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<IdentityUser> userManager;

        [BindProperty]
        public Register RegisterViewModel { get; set; }

        public RegisterModel(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            User user = new User()
            {
                UserName = RegisterViewModel.Email,
                Email = RegisterViewModel.Email,
                FirstName = RegisterViewModel.FirstName,
                LastName = RegisterViewModel.LastName,
            };

            var IdentityResult = await userManager.CreateAsync(user, RegisterViewModel.Password);

            if (IdentityResult.Succeeded)
            {
                var addRolesResult = await userManager.AddToRoleAsync(user, "User");
                await userManager.AddToRoleAsync(user, "SuperAdmin");

                ViewData["Notification"] = new Notification()
                {
                    Type = Classes.Enums.ENNotificationType.Success,
                    Message = "User sucessfully Register."
                };

                return Page();
            }

            string stMessage = "";

            if (IdentityResult.Errors.Count() > 0)
            {
                foreach (var item in IdentityResult.Errors)
                {
                    if (!string.IsNullOrEmpty(item.Description))
                        stMessage += "<br/>";

                    stMessage += item.Description;
                }
            }

            ViewData["Notification"] = new Notification()
            {
                Type = Classes.Enums.ENNotificationType.Error,
                Message = stMessage
            };

            return Page();
        }
    }
}
