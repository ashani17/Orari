using Orari.Models;

namespace Orari.Interfaces
{
    public interface IRoomRepository
    {
        Task<IEnumerable<Rooms>> GetAllRooms();
        Task<Rooms> GetRoomByIdAsync(int id);
        Task<Rooms?> GetRoomByNameAsync(string name);
        Task<Rooms> CreateRoomAsync(Rooms room);
        Task<Rooms> UpdateRoomAsync(Rooms room);
        Task<bool> DeleteRoomAsync(int id);
    }
}
