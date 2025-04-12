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
        public Task<Rooms> CreateRoomAsync(Rooms room)
        {
            _context.Rooms.Add(room);
            return _context.SaveChangesAsync().ContinueWith(t => t.Result > 0 ? room : null);
        }

        public Task<bool> DeleteRoomAsync(int id)
        {
            var room = _context.Rooms.Find(id);
            if (room == null) return Task.FromResult(false);
            _context.Rooms.Remove(room);
            return _context.SaveChangesAsync().ContinueWith(t => t.Result > 0);
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

        public Task<Rooms> GetRoomByIdAsync(int id)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.RId == id);
            if (room == null) throw new Exception("Room not found");
            return Task.FromResult(room);
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
