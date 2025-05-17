using Orari.Models;

namespace Orari.Interfaces
{
    public interface IChatService
    {
        Task<IEnumerable<Chats>> GetAllChats();
        Task<Chats> GetChatByIdAsync(int id);
        Task<Chats> CreateChatAsync(int adminId, string professorEmail);
        Task<bool> DeleteChatAsync(int id);
        Task<List<Chats>> GetChatsByAdminAsync(int adminId);
        Task<List<Chats>> GetChatsByProfesorAsync(int profesorId);
        Task<bool> IsChatParticipantAsync(int chatId, int userId);
        Task<Chats?> GetChatByParticipantsAsync(int adminId, int profesorId);
    }
} 