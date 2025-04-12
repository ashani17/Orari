using Microsoft.AspNetCore.Mvc;
using Orari.DataDbContext;
using Orari.Interfaces;
using Orari.Models;

namespace Orari.Controllers
{
    [Route ("api/rooms")]
    public class RoomController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IRoomRepository _roomRepository;

        public RoomController(AppDbContext context, IRoomRepository roomRepository)
        {
            _context = context;
            _roomRepository = roomRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var rooms = await _roomRepository.GetAllRooms();
            return Ok(rooms);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoomById(int id)
        {
            var room = await _roomRepository.GetRoomByIdAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            return Ok(room);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Rooms room)
        {
            if (room == null)
            {
                return BadRequest();
            }
            var createdRoom = await _roomRepository.CreateRoomAsync(room);
            return CreatedAtAction(nameof(GetRoomById), new { id = createdRoom.RId }, createdRoom);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Rooms room)
        {
            if (room == null)
            {
                return BadRequest();
            }
            var existingRoom = await _roomRepository.GetRoomByIdAsync(id);
            if (existingRoom == null)
            {
                return NotFound();
            }
            existingRoom.RName = room.RName;
            existingRoom.RCapacity = room.RCapacity;
            existingRoom.RType = room.RType;
            existingRoom.RDescription = room.RDescription;
            await _roomRepository.UpdateRoomAsync(existingRoom);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var room = await _roomRepository.GetRoomByIdAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            await _roomRepository.DeleteRoomAsync(id);
            return NoContent();
        }
    }
}
