using JwtAuthWebAPiProject.DTOs;
using JwtAuthWebAPiProject.Models;
using System.Security.Claims;

namespace JwtAuthWebAPiProject.Services.Impl
{
  
        public interface IAuthService
        {

        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
        TokenResponse CreateToken(User user);
        void CreatePaswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token);
    }
    }

