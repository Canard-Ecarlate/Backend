using DuckCity.Domain;
using DuckCity.Domain.Configuration;
using DuckCity.Domain.Games;
using DuckCity.Infrastructure.Repositories;

namespace DuckCity.Application
{
    public class RoomService
    {
        private readonly UserRepository _userRepository;
        private readonly RoomRepository _roomRepository;

        public RoomService(UserRepository userRepository, RoomRepository roomRepository)
        {
            _userRepository = userRepository;
            _roomRepository = roomRepository;
        }

        public Room AddRooms(string? roomName, string? hostId, GameConfiguration? gameConfiguration, bool isPrivate)
        {
            CheckValid.CreateRoom(_userRepository, roomName, hostId, gameConfiguration, false);
            
            string? hostName = _userRepository.GetById(hostId)[0].Name;
            Room room = new()
            {
                Id = Guid.NewGuid().ToString(),
                Name = roomName,
                HostId = hostId,
                HostName = hostName,
                GameConfiguration = gameConfiguration,
                Players = new HashSet<PlayerInRoom>(),
                IsPrivate = isPrivate
            };
            _roomRepository.Create(room);
            return room;
        }

        public Room JoinRooms(string? roomId, string? userId)
        {
            CheckValid.JoinRoom(_roomRepository, _userRepository, userId, roomId);
            
            Room room = _roomRepository.FindById(roomId)[0];
            User user = _userRepository.GetById(userId)[0];
            PlayerInRoom player = new() {Id = userId, Name = user.Name};
            room.Players?.Add(player);
            return room;
        }
    }
}