using System.ComponentModel.DataAnnotations;

namespace DevDiaries.Web.Models.ViewModels
{
    public class AddUser
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
        public bool AdminCheckbox { get; set; }
    }
}
