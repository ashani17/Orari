using Orari.Models;

namespace Orari.Interfaces
{
    public interface IChatRepository
    {
        Task<IEnumerable<Chats>> GetAllChats();
        Task<Chats> GetChatByIdAsync(int id);
        Task<Chats?> GetChatByParticipantsAsync(int adminId, int professorId);
        Task<List<Chats>> GetChatsByAdminAsync(int adminId);
        Task<List<Chats>> GetChatsByProfesorAsync(int profesorId);
        Task<Chats> CreateChatAsync(Chats chat);
        Task<bool> DeleteChatAsync(int id);
        Task<bool> IsChatParticipantAsync(int chatId, int userId);
    }
} 