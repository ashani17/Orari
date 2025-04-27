using Microsoft.AspNetCore.Mvc;
using Orari.DataDbContext;
using Orari.DTO.RoomDTO;
using Orari.Interfaces;
using Orari.Models;

namespace Orari.Controllers
{
    [Route ("api/rooms")]
    public class RoomController : Controller
    {
        
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            
            _roomService = roomService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var rooms = await _roomService.GetAllRooms();
            return Ok(rooms);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoomById(int id)
        {
            var room = await _roomService.GetRoomByIdAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            return Ok(room);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PostRoomDTO room)
        {
            if (room == null)
            {
                return BadRequest();
            }
            // Map the DTO to the entity model
            var roomModel = new Rooms
            {
                RName = room.RName,
                RCapacity = room.RCapacity,
                RType = room.RType,
                RDescription = room.RDescription
            };
            var createdRoom = await _roomService.CreateRoomAsync(roomModel);
            return CreatedAtAction(nameof(GetRoomById), new { id = createdRoom.RId }, createdRoom);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PutRoomDTO room)
        {
            if (room == null)
            {
                return BadRequest();
            }
            var existingRoom = await _roomService.GetRoomByIdAsync(id);
            if (existingRoom == null)
            {
                return NotFound();
            }
            existingRoom.RName = room.RName;
            existingRoom.RCapacity = room.RCapacity;
            existingRoom.RType = room.RType;
            existingRoom.RDescription = room.RDescription;
            await _roomService.UpdateRoomAsync(existingRoom);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var room = await _roomService.GetRoomByIdAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            await _roomService.DeleteRoomAsync(id);
            return NoContent();
        }
    }
}
