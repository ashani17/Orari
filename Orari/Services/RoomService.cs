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

        public async Task<Rooms> CreateRoomAsync(Rooms room)
        {
            var existingRoom = await _roomRepository.GetRoomByNameAsync(room.RName);
            if (existingRoom != null)
            {
                throw new Exception("Room already exists");
            }
            return await _roomRepository.CreateRoomAsync(room);
        }
        public async Task<bool> DeleteRoomAsync(int id)
        {
            var existingRoom = await _roomRepository.GetRoomByIdAsync(id);
            if (existingRoom == null)
            {
                throw new Exception("Room not found");
            }
            return await _roomRepository.DeleteRoomAsync(id);
        }

        public async Task<IEnumerable<Rooms>> GetAllRooms()
        {
            return await _roomRepository.GetAllRooms();
        }

        public async Task<Rooms> GetRoomByIdAsync(int id)
        {
            var room = await _roomRepository.GetRoomByIdAsync(id);
            if (room == null)
            {
                throw new Exception("Room not found");
            }
            return await _roomRepository.GetRoomByIdAsync(id);
        }

        public async Task<Rooms?> GetRoomByNameAsync(string name)
        {
            var room = await _roomRepository.GetRoomByNameAsync(name);
            if (room == null)
            {
                throw new Exception("Room not found");
            }
            return await _roomRepository.GetRoomByNameAsync(name);
        }

        public async Task<Rooms> UpdateRoomAsync(Rooms room)
        {
            return await _roomRepository.UpdateRoomAsync(room);
        }
    }
}
