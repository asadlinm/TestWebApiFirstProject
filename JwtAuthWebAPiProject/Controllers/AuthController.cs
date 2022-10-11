using JwtAuthWebAPiProject.Abstractions;
using JwtAuthWebAPiProject.DTOs;
using JwtAuthWebAPiProject.Models;
using JwtAuthWebAPiProject.Services.Impl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace JwtAuthWebAPiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;
        private readonly IMemoryCache _memoryCache;


        public AuthController(IUserRepository userRepository, IAuthService authService, IMemoryCache memoryCache)
        {
            _userRepository = userRepository;
            _authService = authService;
            _memoryCache = memoryCache;
        }
        [HttpPost]
        public async Task<ActionResult<TokenResponse>> Login(LoginRequest request)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email);

            if (user == null)
            {
                return NotFound("Username or password is not correct");
            }
            if (!_authService.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return NotFound("Username or password is not correct");
            }

            TokenResponse response = _authService.CreateToken(user);
            user.RefreshToken = response.RefreshToken;
            user.RefreshTokenExpireDate = response.RefreshTokenExpireDate;
            await _userRepository.UpdateAsync(user);

            return Ok(response);
        }
        [HttpPost]
        [Route("RefreshToken")]
        public async Task<ActionResult<TokenResponse>> RefreshToken(RefreshTokenRequest request)
        {
            if (request is null)
            {
                return BadRequest("Invalid client request");
            }

            string? accessToken = request.AccessToken;
            string? refreshToken = request.RefreshToken;

            var principal = _authService.GetPrincipalFromExpiredToken(accessToken);
            if (principal == null)
            {
                return BadRequest("Invalid access token or refresh token");
            }
            // get user email from claimTypes and found user
            var user = await _userRepository.GetUserByEmailAsync(principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpireDate <= DateTime.Now)
            {
                return BadRequest("Invalid access token or refresh token");
            }

            var newTokenOutputModel = _authService.CreateToken(user);
            user.RefreshToken = newTokenOutputModel.RefreshToken;
            user.RefreshTokenExpireDate = newTokenOutputModel.RefreshTokenExpireDate;

            await _userRepository.UpdateAsync(user);

            return Ok(newTokenOutputModel);



        }

    }
}
