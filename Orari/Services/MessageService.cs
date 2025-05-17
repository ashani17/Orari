using Microsoft.EntityFrameworkCore;
using Orari.DataDbContext;
using Orari.Interfaces;
using Orari.Models;
using Orari.Constants;

namespace Orari.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IChatService _chatService;

        public MessageService(IMessageRepository messageRepository, IChatService chatService)
        {
            _messageRepository = messageRepository;
            _chatService = chatService;
        }

        public async Task<IEnumerable<Messages>> GetAllMessages()
        {
            return await _messageRepository.GetAllMessages();
        }

        public async Task<Messages> GetMessageByIdAsync(int id)
        {
            return await _messageRepository.GetMessageByIdAsync(id);
        }

        public async Task<Messages> CreateMessageAsync(int chatId, int senderId, string content, string senderRole)
        {
            // Validate chat participant
            if (!await _chatService.IsChatParticipantAsync(chatId, senderId))
                throw new Exception("User is not a participant in this chat");

            var message = new Messages
            {
                CHId = chatId,
                SenderId = senderId,
                Content = content,
                SenderRole = senderRole,
                SentAt = DateTime.UtcNow,
                IsRead = false
            };

            return await _messageRepository.CreateMessageAsync(message);
        }

        public async Task<bool> DeleteMessageAsync(int id)
        {
            return await _messageRepository.DeleteMessageAsync(id);
        }

        public async Task<List<Messages>> GetMessagesByChatAsync(int chatId)
        {
            return await _messageRepository.GetMessagesByChatAsync(chatId);
        }

        public async Task<List<Messages>> GetUnreadMessagesAsync(int chatId, int userId)
        {
            // Validate chat participant
            if (!await _chatService.IsChatParticipantAsync(chatId, userId))
                throw new Exception("User is not a participant in this chat");

            return await _messageRepository.GetUnreadMessagesAsync(chatId, userId);
        }

        public async Task<bool> MarkMessagesAsReadAsync(int chatId, int userId)
        {
            // Validate chat participant
            if (!await _chatService.IsChatParticipantAsync(chatId, userId))
                throw new Exception("User is not a participant in this chat");

            var unreadMessages = await _messageRepository.GetUnreadMessagesAsync(chatId, userId);
            if (!unreadMessages.Any())
                return true;

            return await _messageRepository.MarkMessagesAsReadAsync(unreadMessages);
        }

        public async Task<int> GetUnreadMessageCountAsync(int chatId, int userId)
        {
            // Validate chat participant
            if (!await _chatService.IsChatParticipantAsync(chatId, userId))
                throw new Exception("User is not a participant in this chat");

            return await _messageRepository.GetUnreadMessageCountAsync(chatId, userId);
        }

        public async Task<bool> ValidateMessageAccessAsync(int messageId, int userId)
        {
            var message = await _messageRepository.GetMessageByIdAsync(messageId);
            if (message == null)
                return false;

            return await _chatService.IsChatParticipantAsync(message.CHId, userId);
        }
    }
} 