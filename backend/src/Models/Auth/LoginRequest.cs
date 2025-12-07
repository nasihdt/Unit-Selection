using System.ComponentModel.DataAnnotations;

namespace UniversityRegistration.Api.Models.Auth
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Username الزامی است")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password الزامی است")]
        public string Password { get; set; } = string.Empty;
    }
}
