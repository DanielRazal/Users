namespace Users_Server.Repositories
{
    public interface IMessageRepository
    {
        Task<Message> AddMessage(Message message);
        Task<List<Message>> GetAllMessages();
        Task DeleteAllMessages();
    }

}