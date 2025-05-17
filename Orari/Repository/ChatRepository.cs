using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orari.Interfaces;
using Orari.Models;
using Orari.DataDbContext;

namespace Orari.Repository
{
    public class ChatRepository : IChatRepository
    {
        private readonly AppDbContext _context;

        public ChatRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Chats>> GetAllChats()
        {
            return await _context.Chats
                .Include(c => c.Messages)
                .ToListAsync();
        }

        public async Task<Chats> GetChatByIdAsync(int id)
        {
            var chat = await _context.Chats
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.CHId == id);
            
            if (chat == null) 
                throw new Exception("Chat not found");
                
            return chat;
        }

        public async Task<Chats?> GetChatByParticipantsAsync(int adminId, int professorId)
        {
            return await _context.Chats
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => 
                    c.AId == adminId && 
                    c.PId == professorId);
        }

        public async Task<List<Chats>> GetChatsByAdminAsync(int adminId)
        {
            return await _context.Chats
                .Include(c => c.Messages)
                .Where(c => c.AId == adminId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Chats>> GetChatsByProfesorAsync(int profesorId)
        {
            return await _context.Chats
                .Include(c => c.Messages)
                .Where(c => c.PId == profesorId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<Chats> CreateChatAsync(Chats chat)
        {
            _context.Chats.Add(chat);
            await _context.SaveChangesAsync();
            return chat;
        }

        public async Task<bool> DeleteChatAsync(int id)
        {
            var chat = await _context.Chats.FindAsync(id);
            if (chat == null) 
                return false;
                
            _context.Chats.Remove(chat);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsChatParticipantAsync(int chatId, int userId)
        {
            var chat = await _context.Chats
                .FirstOrDefaultAsync(c => c.CHId == chatId);
            return chat != null && (chat.AId == userId || chat.PId == userId);
        }
    }
} 