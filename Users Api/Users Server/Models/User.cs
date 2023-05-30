using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Users_Server.Enum;

namespace Users_Server.Models
{

    [Table("User")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.USER;
    }
}
