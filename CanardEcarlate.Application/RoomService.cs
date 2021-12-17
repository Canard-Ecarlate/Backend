using CanardEcarlate.Infrastructure.Cache;
using CanardEcarlate.Domain.Games;
using CanardEcarlate.Domain.Configuration;
using CanardEcarlate.Infrastructure.Repositories;
using CanardEcarlate.Application.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System;
using MongoDB.Bson;
using CanardEcarlate.Domain;

namespace CanardEcarlate.Application
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
        public Room AddRooms(string roomName, string hostId, GameConfiguration gameConfiguration, bool isPrivate)
        {
            CheckValidCreateRoom(roomName, hostId, gameConfiguration, false);
            string HostName = _userRepository.GetById(hostId)[0].Name;
            Room room = new Room
            {
                Id = Guid.NewGuid().ToString(),
                Name = roomName,
                HostId = hostId,
                HostName = HostName,
                GameConfiguration = gameConfiguration,
                Players = new HashSet<PlayerInRoom>(),
                IsPrivate = isPrivate
            };
            _roomRepository.Create(room);
            return room;
        }

        public Room JoinRooms(string roomId, string userId)
        {
            CheckValidJoinRoom(userId, roomId);
            Room room = _roomRepository.GetById(roomId)[0];
            User user = _userRepository.GetById(roomId)[0];
            PlayerInRoom player = new PlayerInRoom { Id = userId, Name = user.Name };
            room.Players.Add(player);
            return room;
        }

        public bool CheckValidCreateRoom(string roomName, string hostId, GameConfiguration gameConfiguration, bool privateRoom)
        {
            if ((roomName != "") && (roomName != null))
            {
                if (ObjectId.TryParse(hostId, out _))
                {
                    if (_userRepository.CountUserById(hostId) != 0)
                    {
                        return true;
                    }
                    else
                    {
                        throw new HostIdNoExistException(hostId);
                    }
                }
                else
                {
                    throw new IdNotValidException(hostId);
                }
            }
            else
            {
                throw new RoomNameNullException();
            }
        }

        public bool CheckValidJoinRoom(string userId, string roomId)
        {
            if (ObjectId.TryParse(userId, out _))
            {
                if (_userRepository.CountUserById(userId) != 0)
                {
                    if (_roomRepository.CountRoomById(roomId) != 0)
                    {
                        if (!_roomRepository.GetAllRooms().Any(r => r.Players.Contains(new PlayerInRoom { Id = userId })))
                        {
                            return true;
                        }
                        else
                        {
                            throw new UserAlreadyInRoomException();
                        }
                    }
                    else
                    {
                        throw new UserIdNoExistException();
                    }
                }
                else
                {
                    throw new RoomIdNoExistException();
                }
            }
            else
            {
                throw new IdNotValidException(userId);
            }
        }
    }
}
