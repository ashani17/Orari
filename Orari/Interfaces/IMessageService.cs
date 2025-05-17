using Orari.Models;

namespace Orari.Interfaces
{
    public interface IMessageService
    {
        Task<IEnumerable<Messages>> GetAllMessages();
        Task<Messages> GetMessageByIdAsync(int id);
        Task<Messages> CreateMessageAsync(int chatId, int senderId, string content, string senderRole);
        Task<bool> DeleteMessageAsync(int id);
        Task<List<Messages>> GetMessagesByChatAsync(int chatId);
        Task<List<Messages>> GetUnreadMessagesAsync(int chatId, int userId);
        Task<bool> MarkMessagesAsReadAsync(int chatId, int userId);
        Task<int> GetUnreadMessageCountAsync(int chatId, int userId);
        Task<bool> ValidateMessageAccessAsync(int messageId, int userId);
    }
} 