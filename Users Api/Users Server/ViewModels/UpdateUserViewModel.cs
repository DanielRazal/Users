namespace Users_Server.ViewModels
{
    public class UpdateUserViewModel
    {
        public UpdateUserDTO UpdateUser { get; set; } = null!;

        public IFormFile Photo { get; set; } = null!;
    }
}