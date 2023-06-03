namespace Users_Server.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UsersDBContext _context;
        public UserRepository(UsersDBContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _context.Users
                .Include(u => u.Messages)
                .ToListAsync();
        }


        public async Task<User> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return user;
            }
            else
            {
                return null!;
            }
        }


        public async Task<User> AddUser(User user)
        {
            if (!await _context.Users.AnyAsync())
            {
                _context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('User', RESEED, 1)");
            }
            user.Id = 0;
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }


        public async Task DeleteAllUsers()
        {
            var allUsers = await _context.Users.ToListAsync();
            _context.Users.RemoveRange(allUsers);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserById(int id)
        {
            var user = await _context.Users
                .Include(u => u.Messages)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (user != null)
                return user;
            else return null!;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user != null)
            {
                return user!;
            }
            else return null!;
        }

        public async Task<User> GetUserByUserName(string userName)
        {
            var user = await _context.Users
                .Include(u => u.Messages)
                .FirstOrDefaultAsync(u => u.UserName == userName);
                
            if (user != null)
            {
                return user!;
            }
            else return null!;
        }


        public async Task<User> UpdateUser(User user, int id)
        {
            var _user = await GetUserById(id);
            if (_user != null)
            {
                _user.FirstName = user.FirstName;
                _user.LastName = user.LastName;
                _user.UserName = user.UserName;
                _user.Password = user.Password;
                _user.PhotoUrl = user.PhotoUrl;
                _user.Role = user.Role;
                await _context.SaveChangesAsync();
                return _user;
            }

            return null!;
        }


        public async Task<bool> Login(User user)
        {
            var _user = await _context.Users
                .FirstOrDefaultAsync(x => x.UserName == user.UserName && x.Password == user.Password);
            if (_user != null)
                return true;
            else
                return false;
        }

        public async Task<bool> UserNameExists(string userName)
        {
            var _user = await _context.Users.AnyAsync(x => x.UserName == userName.ToLower());
            return _user;
        }

        public async Task<bool> EmailExists(string email)
        {
            var _user = await _context.Users.AnyAsync(x => x.Email == email.ToLower());
            return _user;
        }

    }
}