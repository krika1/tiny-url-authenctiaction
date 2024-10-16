﻿using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TinyUrl.AuthenticationService.Infrastructure.Common;
using TinyUrl.AuthenticationService.Infrastructure.Contracts.Requests;
using TinyUrl.AuthenticationService.Infrastructure.Contracts.Responses;
using TinyUrl.AuthenticationService.Infrastructure.Entities;
using TinyUrl.AuthenticationService.Infrastructure.Exceptions;
using TinyUrl.AuthenticationService.Infrastructure.Mapping;
using TinyUrl.AuthenticationService.Infrastructure.Repositories;
using TinyUrl.AuthenticationService.Infrastructure.Services;

namespace TinyUrl.AuthenticationService.Bussiness.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserLimitRepository _userLimitRepository;

        public AuthenticationService(IUserRepository userRepository, IUserLimitRepository userLimitRepository)
        {
            _userRepository = userRepository;
            _userLimitRepository=userLimitRepository;
        }

        public async Task<TokenContract> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email!).ConfigureAwait(false);
            if (user == null || !IsPasswordCorrect(request.Password!, user.Password!))
            {
                throw new UnauthorizedException(ErrorMessages.LoginFailedErrorMessage);
            }

            var token = GenerateJwtToken(user!);
            var expiresInLocal = TimeZoneInfo.ConvertTimeFromUtc(token.ValidTo, TimeZoneInfo.Local);

            return new TokenContract
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresIn = expiresInLocal
            };
        }

        public async Task RegisterUserAsync(RegisterUserRequest request)
        {
            var isUserExistis = await _userRepository.CheckIfUserExistsAsync(request.Username!, request.Email!).ConfigureAwait(false);
            if (isUserExistis)
            {
                throw new ConflictException(ErrorMessages.UserAlreadyExistsErrorMessage);
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var user = UserMapping.ToDomain(request);
            user.Password = hashedPassword;

            await _userRepository.CreateUserAsync(user).ConfigureAwait(false);
            await _userLimitRepository.CrateRecordAsync(new UserLimit { UserId = user.Id, DailyCalls = 0 });
        }

        private bool IsPasswordCorrect(string passwordInput, string hashedPassword)
            => BCrypt.Net.BCrypt.Verify(passwordInput, hashedPassword);

        private JwtSecurityToken GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username!),
                new Claim("UserId", user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("qqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqq"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(1),
                signingCredentials: creds);

            return token;
        }
    }
}
