using Orari.Interfaces;
using Orari.Models;

namespace Orari.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public Task<Rooms> CreateRoomAsync(Rooms room)
        {
            var existingRoom = _roomRepository.GetRoomByIdAsync(room.RId);
            if (existingRoom == null)
            {
                throw new Exception("Room already exists");
            }
            return _roomRepository.CreateRoomAsync(room);
        }
        public Task<bool> DeleteRoomAsync(int id)
        {
            var existingRoom = _roomRepository.GetRoomByIdAsync(id);
            if (existingRoom == null)
            {
                throw new Exception("Room not found");
            }
            return _roomRepository.DeleteRoomAsync(id);
        }

        public Task<IEnumerable<Rooms>> GetAllRooms()
        {
            return _roomRepository.GetAllRooms();
        }

        public Task<Rooms> GetRoomByIdAsync(int id)
        {
            var room = _roomRepository.GetRoomByIdAsync(id);
            if (room == null)
            {
                throw new Exception("Room not found");
            }
            return _roomRepository.GetRoomByIdAsync(id);
        }

        public Task<Rooms> UpdateRoomAsync(Rooms room)
        {
            return _roomRepository.UpdateRoomAsync(room);
        }
    }
}
