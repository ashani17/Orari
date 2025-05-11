using Microsoft.EntityFrameworkCore;
using Orari.DataDbContext;
using Orari.Interfaces;
using Orari.Models;

namespace Orari.Repository
{
    public class RoomRepository : IRoomRepository
    {
        private readonly AppDbContext _context;
        public RoomRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Rooms> CreateRoomAsync(Rooms room)
        {
            _context.Rooms.Add(room);
            return await _context.SaveChangesAsync().ContinueWith(t => t.Result > 0 ? room : null);
        }

        public async Task<bool> DeleteRoomAsync(int id)
        {
            var room = await _context.Rooms.FindAsync(id); // Use FindAsync for async operation
            if (room == null) return false; // Return a boolean directly
            _context.Rooms.Remove(room);
            return await _context.SaveChangesAsync() > 0; // Await the async operation and return a boolean
        }

        public Task<IEnumerable<Rooms>> GetAllRooms()
        {
            var rooms = _context.Rooms.ToList();
            if (!rooms.Any())
            {
                return Task.FromResult<IEnumerable<Rooms>>(new List<Rooms>());
            }
            return Task.FromResult<IEnumerable<Rooms>>(rooms);
        }

        public async Task<Rooms> GetRoomByIdAsync(int id)
        {
            return await _context.Rooms.FirstOrDefaultAsync(r => r.RId == id);
        }

        public async Task<Rooms?> GetRoomByNameAsync(string name)
        {
            return await _context.Rooms.FirstOrDefaultAsync(r => r.RName == name);
        }

        public Task<Rooms> UpdateRoomAsync(Rooms room)
        {
            var existingRoom = _context.Rooms.Find(room.RId);
            if (existingRoom == null) throw new Exception("Room not found");
            existingRoom.RName = room.RName;
            return _context.SaveChangesAsync().ContinueWith(t => t.Result > 0 ? existingRoom : null);
        }
    }
}
