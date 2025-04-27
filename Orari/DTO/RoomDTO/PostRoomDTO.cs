namespace Orari.DTO.RoomDTO
{
    public class PostRoomDTO
    {
        public PostRoomDTO()
        {
        }
        public PostRoomDTO(string rName, string rType,string rDescription, int rCapacity)
        {
            RName = rName;
            RType = rType;
            RDescription = rDescription;
            RCapacity = rCapacity;

        }
        public string RName { get; set; } = string.Empty;
        public string RType { get; set; } = string.Empty;
        public int RCapacity { get; set; }
        public string RDescription { get; set; } = string.Empty;
    }
}
