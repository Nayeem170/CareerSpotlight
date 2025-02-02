﻿namespace CareerSpotlightBackend.Models.Settings
{
    public class JwtSettings
    {
        public string? Key { get; set; }
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public int? AccessTokenExpiryMinutes { get; set; }
        public int? RefreshTokenExpiryDays { get; set; }
    }
}
