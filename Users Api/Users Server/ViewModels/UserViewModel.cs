namespace Users_Server.ViewModels
{
    public class UserViewModel
    {
        public UserDTO UserDTO{ get; set; } = null!;

        public IFormFile Photo { get; set; } = null!;
    }
}