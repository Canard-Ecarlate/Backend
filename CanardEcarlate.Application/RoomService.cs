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
        public Room AddRooms(string roomName, string hostId, GameConfiguration gameConfiguration,bool isPrivate) {
            CheckValidRoom(roomName, hostId, gameConfiguration,false);
            string HostName = _userRepository.GetById(hostId)[0].Id;
            Room room = new Room
            {
                Id = Guid.NewGuid().ToString(),
                Name = roomName,
                HostId = hostId,
                HostName = HostName,
                GameConfiguration = gameConfiguration,
                Players = new List<PlayerInRoom>(),
                IsPrivate = isPrivate
            };
            Variables.Rooms.Add(room);
            return room;
        }

        public bool CheckValidRoom(string roomName, string hostId, GameConfiguration gameConfiguration,bool privateRoom) {
                if ((roomName != "") && (roomName != null))
                {
                    if (_userRepository.CountUserById(hostId) != 0)
                    {
                        return true;
                    }
                    else
                    {
                        throw new HostNameNoExistException(hostId);
                    }
                }
                else
                {
                    throw new RoomNameNullException();
                }
            }        
    }
}
