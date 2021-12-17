using System.Linq;
using CanardEcarlate.Application.Exceptions;
using CanardEcarlate.Domain.Configuration;
using CanardEcarlate.Domain.Games;
using CanardEcarlate.Infrastructure.Repositories;
using MongoDB.Bson;

namespace CanardEcarlate.Application
{
    public static class CheckValid
    {
        public static void CreateRoom(UserRepository userRepository, string roomName, string hostId, GameConfiguration gameConfiguration,
            bool privateRoom)
        {
            if (string.IsNullOrEmpty(roomName)) throw new RoomNameNullException();
            if (!ObjectId.TryParse(hostId, out _)) throw new IdNotValidException(hostId);
            if (userRepository.CountUserById(hostId) == 0) throw new HostIdNoExistException(hostId);
        }

        public static void JoinRoom(RoomRepository roomRepository, UserRepository userRepository, string userId, string roomId)
        {
            if (!ObjectId.TryParse(userId, out _)) throw new IdNotValidException(userId);
            if (userRepository.CountUserById(userId) == 0) throw new RoomIdNoExistException();
            if (roomRepository.CountRoomById(roomId) == 0) throw new UserIdNoExistException();
            if (roomRepository.GetAllRooms().Any(r => r.Players.Contains(new PlayerInRoom {Id = userId})))
                throw new UserAlreadyInRoomException();
        }
    }
}