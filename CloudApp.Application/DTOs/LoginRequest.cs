using System.ComponentModel.DataAnnotations;

namespace CloudApp.Application.DTOs
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@(gmail\.com|epam\.com)$",
            ErrorMessage = "Invalid mail domain")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
