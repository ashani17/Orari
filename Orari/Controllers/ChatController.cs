using Microsoft.AspNetCore.Mvc;
using Orari.DataDbContext;
using Orari.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Orari.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        public ChatController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/chat/conversation?user1Id=...&user2Id=...
        [HttpGet("conversation")]
        public async Task<IActionResult> GetConversation(string user1Id, string user2Id)
        {
            var messages = await _context.ChatMessages
                .Where(m => (m.SenderId == user1Id && m.ReceiverId == user2Id) ||
                            (m.SenderId == user2Id && m.ReceiverId == user1Id))
                .OrderBy(m => m.Timestamp)
                .ToListAsync();
            return Ok(messages);
        }

        // POST: api/chat/send
        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] ChatMessage message)
        {
            message.Timestamp = DateTime.UtcNow;
            _context.ChatMessages.Add(message);
            await _context.SaveChangesAsync();
            return Ok(message);
        }

        // GET: api/chat/professor-conversation/{professorId}
        [HttpGet("professor-conversation/{professorId}")]
        public async Task<IActionResult> GetProfessorConversation(string professorId)
        {
            // Get all admin user IDs
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            var adminIds = admins.Select(a => a.Id).ToList();

            var messages = await _context.ChatMessages
                .Where(m =>
                    (m.SenderId == professorId && adminIds.Contains(m.ReceiverId)) ||
                    (adminIds.Contains(m.SenderId) && m.ReceiverId == professorId)
                )
                .OrderBy(m => m.Timestamp)
                .ToListAsync();

            return Ok(messages);
        }
    }
} 