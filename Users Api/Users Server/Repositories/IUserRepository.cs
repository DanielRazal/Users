namespace Users_Server.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsers();
        Task<User> GetUserById(int id);
        Task<User> DeleteUser(int id);
        Task<User> AddUser(User user);
        Task<User> UpdateUser(User user, int id);
        Task DeleteAllUsers();
        Task<bool> Login(User user);
        Task<bool> UserNameExists(string userName);
        Task<bool> EmailExists(string email);
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserByUserName(string userName);
    }
}