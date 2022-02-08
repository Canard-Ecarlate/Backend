﻿using System.Security.Cryptography;
using DuckCity.Application.Services.Interfaces;
using DuckCity.Application.Services.Utils;
using DuckCity.Application.Validations;
using DuckCity.Domain.Exceptions;
using DuckCity.Domain.Users;
using DuckCity.Infrastructure.Repositories;

namespace DuckCity.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public void DeleteAccountUser(string userId)
    {
        CheckValid.ExistUser(_userRepository, userId);
        _userRepository.DeleteUserById(userId);
    }

    public void ChangePasswordUser(string userId, string actualPassword, string newPassword,
        string newPasswordConfirmation)
    {
        IList<User> users = _userRepository.GetById(userId ?? throw  new BadUserOrPasswordException());
        User user = users[0];
        UserUtils.ComparePassword(user,actualPassword);
        if (newPassword == newPasswordConfirmation)
        {
            string encryptedPassword = UserUtils.HashPassword(newPassword);
            user.Password = encryptedPassword;
            _userRepository.Replace(user);
        }
        else
        {
            throw new PasswordConfirmationException();
        }
        
    }
}