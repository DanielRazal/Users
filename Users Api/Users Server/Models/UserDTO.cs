using System.ComponentModel.DataAnnotations;
using Users_Server.Attributes;

namespace Users_Server.Models
{
    public class UserDTO
    {
        [LengthRange(2, 50, ErrorMessage = "{0} must be between {1} and {2} characters.")]
        public string FirstName { get; set; } = string.Empty;

        [LengthRange(2, 50, ErrorMessage = "{0} must be between {1} and {2} characters.")]
        public string LastName { get; set; } = string.Empty;
        [LengthRange(2, 50, ErrorMessage = "{0} must be between {1} and {2} characters.")]
        public string UserName { get; set; } = string.Empty;
        [LengthRange(2, 50, ErrorMessage = "{0} must be between {1} and {2} characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).{8,}$",
           ErrorMessage = "Password must contain at least 8 characters including uppercase, lowercase, numeric, and special characters.")]
        public string Password { get; set; } = string.Empty;
        [LengthRange(2, 50, ErrorMessage = "{0} must be between {1} and {2} characters.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;

        [Range(typeof(bool), "true", "true", ErrorMessage = "You must accept the terms and conditions.")]
        public bool AcceptedTerms { get; set; }

    }
}