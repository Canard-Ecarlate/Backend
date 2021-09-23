using CanardEcarlate.Domain;
using CanardEcarlate.Infrastructure;
using CanardEcarlate.Infrastructure.Repositories;
using MongoDB.Driver;
using System.Collections.Generic;

namespace CanardEcarlate.Application
{
    public class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public List<User> Get() =>
            _userRepository.Get();

        public User Get(string id) =>
            _userRepository.Get(id);

        public User Create(User user)
        {
            _userRepository.Create(user);
            return user;
        }

        public void Update(string id, User userIn) =>
            _userRepository.Update(id, userIn);

        public void Remove(User user) =>
            _userRepository.Remove(user);

        public void Remove(string id) =>
            _userRepository.Remove(id);
    }
}
