using CanardEcarlate.Domain.Games;
using CanardEcarlate.Domain.Configuration;
using CanardEcarlate.Infrastructure.Repositories;
using System.Collections.Generic;
using System;
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
            CheckValid.CreateRoom(_userRepository, roomName, hostId, gameConfiguration, false);
            
            string hostName = _userRepository.GetById(hostId)[0].Name;
            Room room = new Room
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

        public Room JoinRooms(string roomId, string userId)
        {
            CheckValid.JoinRoom(_roomRepository, _userRepository, userId, roomId);
            
            Room room = _roomRepository.GetById(roomId)[0];
            User user = _userRepository.GetById(userId)[0];
            PlayerInRoom player = new PlayerInRoom {Id = userId, Name = user.Name};
            room.Players.Add(player);
            return room;
        }

    }
}