﻿using CosmoVerse.Models.Domain;
using CosmoVerse.Models.Dto;
using CosmoVerse.Services.Results;
using Microsoft.AspNetCore.Mvc;

namespace CosmoVerse.Services
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(UserDto request);
        Task<AuthResult> LoginAsync(UserLoginDto request);
        Task<UserInfoDto?> GetUserAsync(Guid Id);
        Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request);
    }
}
