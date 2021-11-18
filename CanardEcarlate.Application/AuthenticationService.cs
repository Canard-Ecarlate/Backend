using System;
using CanardEcarlate.Domain;
using CanardEcarlate.Infrastructure.Repositories;
using System.Collections.Generic;
using System.Security.Cryptography;
using CanardEcarlate.Application.Exceptions;

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
        
        public User Login(string name, string password)
        {
            IList<User> users = _userRepository.GetByName(name);

            if (users.Count != 1)
            {
                throw new UnauthorizedAccessException();
            }
            
            User user = users[0];
            /* Extract the bytes */
            byte[] hashBytes = Convert.FromBase64String(user.Password);
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
            return user;
        }

        public void SignUp(string name, string email, string password, string passwordConfirmation)
        {
            if (_userRepository.CountUserByName(name) == 0)
            {
                if (_userRepository.CountUserByEmail(email) == 0)
                {
                    if (password == passwordConfirmation)
                    {
                        string encryptedPassword = HashPassword(password);
                        User user = new User {Name = name, Email = email, Password = encryptedPassword};
                        _userRepository.Create(user);
                    }
                    else
                    {
                        throw new PasswordConfirmationException();
                    }
                }
                else
                {
                    throw new MailAlreadyExistException(email);
                }
            }
            else
            {
                throw new UsernameAlreadyExistException(name);
            }
        }
        
        private static string HashPassword(string password)
        {
            byte[] salt = new byte[HASH_SIZE];
            new RNGCryptoServiceProvider().GetBytes(salt);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, ITERATIONS);
            byte[] hash = pbkdf2.GetBytes(BYTE_SIZE);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, HASH_SIZE);
            Array.Copy(hash, 0, hashBytes, HASH_SIZE, BYTE_SIZE);
            string savedPasswordHash = Convert.ToBase64String(hashBytes);
            return savedPasswordHash;
        }
    }
}