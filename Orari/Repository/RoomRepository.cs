using Orari.Interfaces;
using Orari.Models;

namespace Orari.Repository
{
    public class RoomRepository : IRoomRepository
    {
        public Task<Rooms> CreateRoomAsync(Rooms room)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteRoomAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Rooms>> GetAllRooms()
        {
            throw new NotImplementedException();
        }

        public Task<Rooms> GetRoomByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Rooms> UpdateRoomAsync(Rooms room)
        {
            throw new NotImplementedException();
        }
    }
}
