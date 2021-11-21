using System.ComponentModel.DataAnnotations;

namespace CorewebAPI.Entities.Authorization
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email User Name is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
