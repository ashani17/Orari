using Microsoft.AspNetCore.Mvc;
using Orari.DataDbContext;
using Orari.DTO.RoomDTO;
using Orari.Interfaces;
using Orari.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Orari.Controllers
{
    [Route ("api/rooms")]
    public class RoomController : Controller
    {
        
        private readonly IRoomService _roomService;
        private readonly AppDbContext _context;

        public RoomController(IRoomService roomService, AppDbContext context)
        {
            
            _roomService = roomService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var rooms = await _roomService.GetAllRooms();
            return Ok(rooms);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoomById([FromRoute] int id)
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
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var room = await _roomService.GetRoomByIdAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            await _roomService.DeleteRoomAsync(id);
            return NoContent();
        }

        // GET: api/rooms/free?date=YYYY-MM-DD&startTime=HH:mm&endTime=HH:mm
        [HttpGet("free")]
        public async Task<IActionResult> GetFreeRooms([FromQuery] string date, [FromQuery] string startTime, [FromQuery] string endTime)
        {
            if (string.IsNullOrEmpty(date) || string.IsNullOrEmpty(startTime) || string.IsNullOrEmpty(endTime))
                return BadRequest("Missing date, startTime, or endTime");

            // Parse date and times
            if (!DateTime.TryParse(date, out var parsedDate))
                return BadRequest("Invalid date format");
            if (!TimeSpan.TryParse(startTime, out var parsedStart))
                return BadRequest("Invalid startTime format");
            if (!TimeSpan.TryParse(endTime, out var parsedEnd))
                return BadRequest("Invalid endTime format");

            var parsedDateOnly = DateOnly.FromDateTime(parsedDate);
            var parsedStartTimeOnly = TimeOnly.FromTimeSpan(parsedStart);
            var parsedEndTimeOnly = TimeOnly.FromTimeSpan(parsedEnd);

            // Get all rooms
            var allRooms = await _context.Rooms.ToListAsync();

            // Get all schedules that overlap with the requested time
            var busyRoomIds = await _context.Schedules
                .Where(s => s.Date == parsedDateOnly &&
                    (s.StartTime < parsedEndTimeOnly && s.EndTime > parsedStartTimeOnly))
                .Select(s => s.RId)
                .ToListAsync();

            // Also check Exams if they use rooms
            var busyExamRoomIds = await _context.Exams
                .Where(e => e.ExamDate.Date == parsedDate.Date &&
                    (e.StartTime < parsedEnd && e.EndTime > parsedStart))
                .Select(e => e.RId)
                .ToListAsync();

            var allBusyRoomIds = busyRoomIds.Concat(busyExamRoomIds).Distinct().ToList();

            var freeRooms = allRooms.Where(r => !allBusyRoomIds.Contains(r.RId)).ToList();

            return Ok(freeRooms);
        }
    }
}
