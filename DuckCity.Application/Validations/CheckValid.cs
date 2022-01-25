using DuckCity.Domain.Exceptions;
using DuckCity.Domain.Rooms;
using DuckCity.Infrastructure.Repositories.Interfaces;
using MongoDB.Bson;

namespace DuckCity.Application.Validations
{
    public static class CheckValid
    {
        public static void CreateRoom(IUserRepository userRepository, string? roomName, string? hostId)
        {
            if (string.IsNullOrEmpty(roomName))
            {
                throw new RoomNameNullException();
            }

            if (!ObjectId.TryParse(hostId, out _))
            {
                throw new IdNotValidException(hostId);
            }

            if (userRepository.CountUserById(hostId) == 0)
            {
                throw new HostIdNoExistException(hostId);
            }
        }

        public static void JoinRoom(IRoomRepository roomRepository, IUserRepository userRepository, string? userId,
            string? roomId)
        {
            if (!ObjectId.TryParse(userId, out _))
            {
                throw new IdNotValidException(userId);
            }

            if (userRepository.CountUserById(userId) == 0)
            {
                throw new UserIdNoExistException();
            }

            if (roomRepository.FindById(roomId) == null)
            {
                throw new RoomIdNoExistException();
            }

            if (roomRepository.FindAllRooms()
                .Any(r => r.Players != null && r.Players.Contains(new PlayerInRoom {Id = userId})))
            {
                throw new UserAlreadyInRoomException(userId);
            }
        }

        public static Room LeaveRoom(IRoomRepository roomRepository, IUserRepository userRepository, string userId,
            string roomId)
        {
            if (!ObjectId.TryParse(userId, out _))
            {
                throw new IdNotValidException(userId);
            }

            if (userRepository.CountUserById(userId) == 0)
            {
                throw new UserIdNoExistException();
            }
            
            Room? room = roomRepository.FindById(roomId);
            
            if (room == null)
            {
                throw new RoomIdNoExistException();
            }

            if (room.Players == null)
            {
                throw new PlayersNotFoundException();
            }

            if (!room.Players.Contains(new PlayerInRoom {Id = userId}))
            {
                throw new UserNotInRoomException(userId);
            }

            return room;
        }
    }
}