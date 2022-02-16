using DuckCity.Domain.Exceptions;
using DuckCity.Domain.Rooms;
using DuckCity.Infrastructure.RoomPreviewRepository;
using DuckCity.Infrastructure.UserRepository;
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
        
        public static void CreateRoom(IRoomPreviewRepository roomPreviewRepository, IUserRepository userRepository, string roomName, string hostId)
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
            
            if (roomPreviewRepository.FindAllRooms()
                .Any(r => r.PlayersId.Contains(hostId)))
            {
                throw new UserAlreadyInRoomException(hostId);
            }
        }

        public static void JoinRoom(IRoomPreviewRepository roomPreviewRepository, IUserRepository userRepository, string userId)
        {
            IsObjectId(userId);
            
            if (userRepository.CountUserById(userId) == 0)
            {
                throw new UserIdNoExistException();
            }
        }

        public static void LeaveRoom(IUserRepository userRepository, string userId, RoomPreview roomPreview)
        {
            IsObjectId(userId);

            if (userRepository.CountUserById(userId) == 0)
            {
                throw new UserIdNoExistException();
            }
            
            if (!roomPreview.PlayersId.Contains(userId))
            {
                throw new UserNotInRoomException(userId);
            }
        }

        public static void ExistUser(IUserRepository userRepository, string? userId)
        {
            if (userId == null)
            {
                throw new IdNotValidException(userId);
            }
            IsObjectId(userId);

            if (userRepository.CountUserById(userId) == 0)
            {
                throw new UserIdNoExistException();
            }
        }

        public static void SignUp(IUserRepository userRepository, string? name, string? email, string? password, string? passwordConfirmation)
        {
            if (password != passwordConfirmation)
            {
                throw new PasswordConfirmationException();
            }
            if (userRepository.CountUserByName(name) != 0)
            {
                throw new UsernameAlreadyExistException(name);
            }
            if (userRepository.CountUserByEmail(email) != 0)
            {
                throw new MailAlreadyExistException(email);
            }
        }
    }
}