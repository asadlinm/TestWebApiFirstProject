﻿namespace JwtAuthWebAPiProject.DTOs
{
    public class TokenResponse
    {
        public string Token { get; set; }
        public DateTime TokenExpireDate { get; set; }
        public int UserId { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpireDate { get; set; }
    }
}
