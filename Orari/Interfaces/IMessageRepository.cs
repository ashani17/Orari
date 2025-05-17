using Orari.Models;

namespace Orari.Interfaces
{
    public interface IMessageRepository
    {
        Task<IEnumerable<Messages>> GetAllMessages();
        Task<Messages> GetMessageByIdAsync(int id);
        Task<List<Messages>> GetMessagesByChatAsync(int chatId);
        Task<Messages> CreateMessageAsync(Messages message);
        Task<bool> DeleteMessageAsync(int id);
        Task<List<Messages>> GetUnreadMessagesAsync(int chatId, int userId);
        Task<bool> MarkMessagesAsReadAsync(IEnumerable<Messages> messages);
        Task<int> GetUnreadMessageCountAsync(int chatId, int userId);
    }
} 