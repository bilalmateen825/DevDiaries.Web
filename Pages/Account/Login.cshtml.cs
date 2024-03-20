using DevDiaries.Web.Classes;
using DevDiaries.Web.Models.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DevDiaries.Web.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> signInManager;

        [BindProperty]
        public Login LoginViewModel { get; set; }

        public LoginModel(SignInManager<IdentityUser> signInManager)
        {
            this.signInManager = signInManager;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(string ReturnURL)
        {
            var signInResult = await signInManager.PasswordSignInAsync(LoginViewModel.Email, LoginViewModel.Password, false, false);

            if (signInResult.Succeeded)
            {
                if (!string.IsNullOrEmpty(ReturnURL))
                    return RedirectToPage(ReturnURL);

                return RedirectToPage("/Index");
            }

            ViewData["Notification"] = new Notification()
            {
                Type = Classes.Enums.ENNotificationType.Error,
                Message = $"Unable to login."
            };


            return Page();
        }
    }
}
