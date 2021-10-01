using System;
using CanardEcarlate.Domain;
using CanardEcarlate.Infrastructure.Repositories;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace CanardEcarlate.Application
{
    public class AuthenticationService
    {
        private readonly UserRepository _userRepository;
        private const int HASH_SIZE = 16;
        private const int ITERATIONS = 100000;
        private const int BYTE_SIZE = 20;

        public AuthenticationService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public List<User> Get() =>
            _userRepository.Get();

        public User Get(string id) =>
            _userRepository.Get(id);

        public User Create(User user)
        {
            user.Password = HashPassword(user.Password);
            _userRepository.Create(user);
            return user;
        }

        public void Update(string id, User userIn) =>
            _userRepository.Update(id, userIn);

        public void Remove(User user) =>
            _userRepository.Remove(user);

        public User Login(string name, string password)
        {
            long nbPseudo = _userRepository.CountUserByName(name);
            
            if (nbPseudo != 1) throw new UnauthorizedAccessException();
            
            User usertemp = _userRepository.GetByName(name);
            /* Extract the bytes */
            byte[] hashBytes = Convert.FromBase64String(usertemp.Password);
            /* Get the salt */
            byte[] salt = new byte[HASH_SIZE];
            Array.Copy(hashBytes, 0, salt, 0, HASH_SIZE);
            /* Compute the hash on the password the user entered */
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, ITERATIONS);
            byte[] hash = pbkdf2.GetBytes(BYTE_SIZE);
            /* Compare the results */
            for (int i = 0; i < BYTE_SIZE; i++)
            {
                if (hashBytes[i + HASH_SIZE] != hash[i])
                {
                    throw new UnauthorizedAccessException();
                }
            }
            return usertemp;
        }

        private string HashPassword(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[HASH_SIZE]);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, ITERATIONS);
            byte[] hash = pbkdf2.GetBytes(BYTE_SIZE);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, HASH_SIZE);
            Array.Copy(hash, 0, hashBytes, HASH_SIZE, BYTE_SIZE);
            string savedPasswordHash = Convert.ToBase64String(hashBytes);
            return savedPasswordHash;
        }

        public void SignUp(string name, string email, string password, string passwordConfirmation)
        {
            if (_userRepository.CountUserByName(name) == 0)
            {
                if (_userRepository.CountUserByMail(email) == 0)
                {
                    if (password == passwordConfirmation)
                    {
                        string encryptedPassword = HashPassword(password);
                        User user = new User {Name = name, Email = email, Password = encryptedPassword};
                        _userRepository.Create(user);
                    }
                    else
                    {
                        throw new Exception("Passwords are not equals");
                    }
                }
                else
                {
                    throw new Exception("Email " + email + " is still used");
                }
            }
            else
            {
                throw new Exception("User name " + name + " is still used");
            }
        }
    }
}