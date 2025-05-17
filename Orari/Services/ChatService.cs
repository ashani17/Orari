using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Orari.DataDbContext;
using Orari.Models;
using Microsoft.EntityFrameworkCore;
using Orari.Constants;
using Orari.Interfaces;

namespace Orari.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IProfesorService _profesorService;

        public ChatService(IChatRepository chatRepository, IProfesorService profesorService)
        {
            _chatRepository = chatRepository;
            _profesorService = profesorService;
        }

        public async Task<IEnumerable<Chats>> GetAllChats()
        {
            return await _chatRepository.GetAllChats();
        }

        public async Task<Chats> GetChatByIdAsync(int id)
        {
            var chat = await _chatRepository.GetChatByIdAsync(id);
            if (chat == null)
                throw new Exception("Chat not found");
            return chat;
        }

        public async Task<Chats> CreateChatAsync(int creatorId, string targetEmail)
        {
            var targetUser = await _profesorService.GetProfesorByEmailAsync(targetEmail);
            if (targetUser == null)
                throw new Exception("Target user not found");

            if (creatorId == targetUser.PId)
                throw new Exception("Cannot create a chat with yourself");

            var existingChat = await _chatRepository.GetChatByParticipantsAsync(creatorId, targetUser.PId);
            if (existingChat != null)
                return existingChat;

            var chat = new Chats
            {
                AId = creatorId,
                PId = targetUser.PId,
                CreatedAt = DateTime.UtcNow
            };

            return await _chatRepository.CreateChatAsync(chat);
        }

        public async Task<bool> DeleteChatAsync(int id)
        {
            return await _chatRepository.DeleteChatAsync(id);
        }

        public async Task<List<Chats>> GetChatsByAdminAsync(int adminId)
        {
            return await _chatRepository.GetChatsByAdminAsync(adminId);
        }

        public async Task<List<Chats>> GetChatsByProfesorAsync(int profesorId)
        {
            return await _chatRepository.GetChatsByProfesorAsync(profesorId);
        }

        public async Task<bool> IsChatParticipantAsync(int chatId, int userId)
        {
            var chat = await _chatRepository.GetChatByIdAsync(chatId);
            return chat != null && (chat.AId == userId || chat.PId == userId);
        }

        public async Task<Chats?> GetChatByParticipantsAsync(int adminId, int profesorId)
        {
            return await _chatRepository.GetChatByParticipantsAsync(adminId, profesorId);
        }
    }
} 