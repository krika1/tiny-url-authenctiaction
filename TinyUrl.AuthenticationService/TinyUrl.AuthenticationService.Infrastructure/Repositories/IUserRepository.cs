﻿using TinyUrl.AuthenticationService.Infrastructure.Entities;

namespace TinyUrl.AuthenticationService.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task CreateUserAsync(User user);
        Task<User> GetUserByEmailAsync(string email);
        Task<bool> CheckIfUserExistsAsync(string userName, string email);
    }
}
