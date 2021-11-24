using CanardEcarlate.Infrastructure.Cache;
using CanardEcarlate.Domain.Games;
using CanardEcarlate.Domain.Configuration;
using CanardEcarlate.Infrastructure.Repositories;
using CanardEcarlate.Application.Exceptions;
using System.Collections.Generic;

namespace CanardEcarlate.Application
{
    public class RoomService
    {
        private readonly UserRepository _userRepository;
        public RoomService(UserRepository userRepository) {
            _userRepository = userRepository;
        }
        public void addRooms(string roomName, string hostName, GameConfiguration gameConfiguration, bool isPrivate) {
            if (!Variables.Rooms.Contains(new Room { Name = roomName }))
            {
                if ((roomName != "") && (roomName != null))
                {
                    if (_userRepository.CountUserByName(hostName) != 0)
                    {
                        Room room = new Room
                        {
                            Name = roomName,
                            HostName = hostName,
                            GameConfiguration = gameConfiguration,
                            Players = new List<PlayerInRoom>(),
                            IsPrivate = isPrivate
                        };
                        Variables.Rooms.Add(room);
                    }
                    else
                    {
                        throw new HostNameNoExistException(hostName);
                    }
                }
                else
                {
                    throw new RoomNameNullException();
                }
            }
            else {
                throw new RoomNameAlreadyExistException();
            }
        }
    }
}
