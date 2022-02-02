using DuckCity.Domain.Exceptions;
using DuckCity.Domain.Rooms;
using DuckCity.Infrastructure.Repositories.Interfaces;
using MongoDB.Bson;

namespace DuckCity.Application.Validations
{
    public static class CheckValid
    {
        public static void IsObjectId(string id)
        {
            if (!ObjectId.TryParse(id, out _))
            {
                throw new IdNotValidException(id);
            }
        }
        
        public static void CreateRoom(IRoomRepository roomRepository, IUserRepository userRepository, string roomName, string hostId)
        {
            IsObjectId(hostId);

            if (string.IsNullOrEmpty(roomName))
            {
                throw new RoomNameNullException();
            }
            
            if (userRepository.CountUserById(hostId) == 0)
            {
                throw new HostIdNoExistException(hostId);
            }
            
            if (roomRepository.FindAllRooms()
                .Any(r => r.PlayersId.Contains(hostId)))
            {
                throw new UserAlreadyInRoomException(hostId);
            }
        }

        public static void JoinRoom(IRoomRepository roomRepository, IUserRepository userRepository, string userId)
        {
            IsObjectId(userId);
            
            if (userRepository.CountUserById(userId) == 0)
            {
                throw new UserIdNoExistException();
            }

            if (roomRepository.FindAllRooms()
                .Any(r => r.PlayersId.Contains(userId)))
            {
                throw new UserAlreadyInRoomException(userId);
            }
        }

        public static void LeaveRoom(IUserRepository userRepository, string userId, Room room)
        {
            IsObjectId(userId);

            if (userRepository.CountUserById(userId) == 0)
            {
                throw new UserIdNoExistException();
            }
            
            if (!room.PlayersId.Contains(userId))
            {
                throw new UserNotInRoomException(userId);
            }
        }
    }
}