using CanardEcarlate.Infrastructure.Cache;
using CanardEcarlate.Domain.Games;
using CanardEcarlate.Domain.Configuration;
using CanardEcarlate.Infrastructure.Repositories;
using CanardEcarlate.Application.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System;

namespace CanardEcarlate.Application
{
    public class RoomService
    {
        private readonly UserRepository _userRepository;
        public RoomService(UserRepository userRepository) {
            _userRepository = userRepository;
        }
        public void AddPublicRooms(string roomName, string hostName, GameConfiguration gameConfiguration) {
            CheckValidRoom(roomName,hostName,gameConfiguration,false);
            Room room = new Room
            {
                Id = Guid.NewGuid().ToString(),
                Name = roomName,
                HostName = hostName,
                GameConfiguration = gameConfiguration,
                Players = new List<PlayerInRoom>(),
                IsPrivate = false
            };
            Variables.PublicRooms.Add(room);
        }

        public void AddPrivateRooms(string roomName, string hostName, GameConfiguration gameConfiguration)
        {
            CheckValidRoom(roomName, hostName, gameConfiguration,true);
            Room room = new Room
            {
                Id = Guid.NewGuid().ToString(),
                Name = roomName,
                HostName = hostName,
                GameConfiguration = gameConfiguration,
                Players = new List<PlayerInRoom>(),
                IsPrivate = true
            };
            Variables.PrivateRooms.Add(room);
        }

        public bool CheckValidRoom(string roomName, string hostName, GameConfiguration gameConfiguration,bool privateRoom) {
            if (!Variables.PublicRooms.Any(room => room.Name == roomName) || privateRoom)
            {
                if ((roomName != "") && (roomName != null))
                {
                    if (_userRepository.CountUserByName(hostName) != 0)
                    {
                        return true;
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
            else
            {
                throw new RoomNameAlreadyExistException();
            }
        }
    }
}
