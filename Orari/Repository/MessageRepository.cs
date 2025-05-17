using Microsoft.EntityFrameworkCore;
using Orari.DataDbContext;
using Orari.Interfaces;
using Orari.Models;

namespace Orari.Repository
{
    public class MessageRepository : IMessageRepository
    {
        private readonly AppDbContext _context;

        public MessageRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Messages>> GetAllMessages()
        {
            return await _context.Messages
                .Include(m => m.Chat)
                .ToListAsync();
        }

        public async Task<Messages> GetMessageByIdAsync(int id)
        {
            var message = await _context.Messages
                .Include(m => m.Chat)
                .FirstOrDefaultAsync(m => m.MId == id);

            if (message == null)
                throw new Exception("Message not found");

            return message;
        }

        public async Task<List<Messages>> GetMessagesByChatAsync(int chatId)
        {
            return await _context.Messages
                .Where(m => m.CHId == chatId)
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }

        public async Task<Messages> CreateMessageAsync(Messages message)
        {
            // Verify that the chat exists
            var chatExists = await _context.Chats.AnyAsync(c => c.CHId == message.CHId);
            if (!chatExists)
                throw new Exception("Chat not found");

            message.SentAt = DateTime.UtcNow;
            message.IsRead = false;

            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<bool> DeleteMessageAsync(int id)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message == null)
                return false;

            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Messages>> GetUnreadMessagesAsync(int chatId, int userId)
        {
            return await _context.Messages
                .Where(m => m.CHId == chatId &&
                           m.SenderId != userId &&
                           !m.IsRead)
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }

        public async Task<bool> MarkMessagesAsReadAsync(IEnumerable<Messages> messages)
        {
            foreach (var message in messages)
            {
                message.IsRead = true;
            }

            var changedRows = await _context.SaveChangesAsync();
            return changedRows > 0;
        }

        public async Task<int> GetUnreadMessageCountAsync(int chatId, int userId)
        {
            return await _context.Messages
                .CountAsync(m => m.CHId == chatId &&
                                m.SenderId != userId &&
                                !m.IsRead);
        }
    }
}