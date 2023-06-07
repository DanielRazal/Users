namespace Users_Server.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly UsersDBContext _context;
        public MessageRepository(UsersDBContext context)
        {
            _context = context;
        }

        public async Task<Message> AddMessage(Message message)
        {
            if (!await _context.Messages.AnyAsync())
            {
                _context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Messages', RESEED, 0)");
            }
            message.Id = 0;
            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<List<Message>> GetAllMessages()
        {
            return await _context.Messages.ToListAsync();
        }

        public async Task DeleteAllMessages()
        {
            var allMessages = await _context.Messages.ToListAsync();
            _context.Messages.RemoveRange(allMessages);
            await _context.SaveChangesAsync();
        }
    }
}