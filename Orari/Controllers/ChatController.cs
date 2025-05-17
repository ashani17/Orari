using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Orari.DTO.ChatsDTO;
using Orari.Services;
using Orari.Constants;
using Orari.Models;
using Orari.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Orari.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Professor")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly IMessageService _messageService;
        private readonly IProfesorService _profesorService;
        private readonly UserManager<IdentityUser> _userManager;

        public ChatController(
            IChatService chatService, 
            IMessageService messageService,
            IProfesorService profesorService,
            UserManager<IdentityUser> userManager)
        {
            _chatService = chatService;
            _messageService = messageService;
            _profesorService = profesorService;
            _userManager = userManager;
        }

        [HttpPost("create")]
        [Authorize(Roles = "Admin,Professor")]
        public async Task<IActionResult> CreateChat([FromBody] CreateChatDTO dto)
        {
            try
            {
                // Get user's email from claims
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value!;
                var targetEmail = dto.TargetEmail;
                
                // Get user's profile to get their ID
                var user = await _profesorService.GetProfesorByEmailAsync(userEmail);
                var targetUser = await _profesorService.GetProfesorByEmailAsync(targetEmail);
                
                if (user == null)
                    return BadRequest("User not found");
                if (targetUser == null)
                    return BadRequest("Target user not found");

                // Check if the target user has the appropriate role
                var targetIdentityUser = await _userManager.FindByEmailAsync(targetEmail);
                if (targetIdentityUser == null)
                    return BadRequest("Target user not found in identity system");

                var isUserAdmin = User.IsInRole(UserRoles.Admin);
                var isTargetAdmin = await _userManager.IsInRoleAsync(targetIdentityUser, UserRoles.Admin);

                // Validate that professors can only chat with admins and vice versa
                if (isUserAdmin == isTargetAdmin)
                    return BadRequest("Chats can only be created between professors and admins");

                // Create the chat with the correct order of IDs
                var chat = isUserAdmin 
                    ? await _chatService.CreateChatAsync(user.PId, targetEmail)  // Admin creating chat with professor
                    : await _chatService.CreateChatAsync(targetUser.PId, userEmail);  // Professor creating chat with admin

                return Ok(chat);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{chatId}/messages")]
        public async Task<IActionResult> SendMessage(int chatId, [FromBody] SendMessageDTO dto)
        {
            try
            {
                // Get sender's email from claims
                var senderEmail = User.FindFirst(ClaimTypes.Email)?.Value!;
                // Get sender's profile to get their ID
                var sender = await _profesorService.GetProfesorByEmailAsync(senderEmail);
                if (sender == null)
                    return BadRequest("Sender not found");

                var senderRole = User.IsInRole(UserRoles.Admin) ? UserRoles.Admin : UserRoles.Professor;
                
                var message = await _messageService.CreateMessageAsync(
                    chatId,
                    sender.PId,
                    dto.Content,
                    senderRole
                );
                return Ok(message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("my-chats")]
        public async Task<IActionResult> GetMyChats()
        {
            try
            {
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value!;
                var user = await _profesorService.GetProfesorByEmailAsync(userEmail);
                if (user == null)
                    return BadRequest("User not found");

                List<Chats> chats;
                if (User.IsInRole(UserRoles.Admin))
                {
                    chats = await _chatService.GetChatsByAdminAsync(user.PId);
                }
                else
                {
                    chats = await _chatService.GetChatsByProfesorAsync(user.PId);
                }
                return Ok(chats);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{chatId}/messages")]
        public async Task<IActionResult> GetChatMessages(int chatId)
        {
            try
            {
                // Get user's email from claims
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value!;
                // Get user's profile to get their ID
                var user = await _profesorService.GetProfesorByEmailAsync(userEmail);
                if (user == null)
                    return BadRequest("User not found");

                // Verify user is part of this chat
                if (!await _chatService.IsChatParticipantAsync(chatId, user.PId))
                    return Forbid("User is not a participant in this chat");

                var messages = await _messageService.GetMessagesByChatAsync(chatId);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("admins")]
        [Authorize(Roles = "Professor")]
        public async Task<IActionResult> GetAvailableAdmins()
        {
            try
            {
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value!;
                var user = await _profesorService.GetProfesorByEmailAsync(userEmail);
                if (user == null)
                    return BadRequest("User not found");

                // Get all users with Admin role
                var admins = await _profesorService.GetAllAdminsAsync();
                return Ok(admins);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
} 