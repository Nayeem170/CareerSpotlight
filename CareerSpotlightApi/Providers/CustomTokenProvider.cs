using Microsoft.AspNetCore.Identity;

namespace CareerSpotlightApi.Providers
{
    using CareerSpotlightApi.Models.Settings;
    using Microsoft.Extensions.Options;
    using System.Security.Cryptography;

    public class CustomTokenProvider<TUser> : IUserTwoFactorTokenProvider<TUser>
        where TUser : class
    {
        private readonly TokenSettings _tokenSettings;

        public CustomTokenProvider(IOptions<TokenSettings> options)
        {
            _tokenSettings = options.Value;
        }

        public Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<TUser> manager, TUser user)
        {
            return Task.FromResult(true);
        }

        private static string GenerateSecureToken()
        {
            byte[] tokenBytes = new byte[4];
            RandomNumberGenerator.Fill(tokenBytes);

            int token = BitConverter.ToInt32(tokenBytes, 0) & 999999;
            return token.ToString("D6");
        }

        public async Task<string> GenerateAsync(string purpose, UserManager<TUser> manager, TUser user)
        {
            string token = GenerateSecureToken();
            string expiryTime = DateTime.UtcNow.AddMinutes(_tokenSettings.TokenExpiryMinutes).ToString("O");

            string tokenData = $"{token}${expiryTime}";
            await manager.SetAuthenticationTokenAsync(user, "Default", purpose, tokenData);

            return token;
        }

        public async Task<bool> ValidateAsync(string purpose, string token, UserManager<TUser> manager, TUser user)
        {
            string storedTokenData = await manager.GetAuthenticationTokenAsync(user, "Default", purpose) ?? string.Empty;
            if (string.IsNullOrEmpty(storedTokenData)) return false;

            string[] parts = storedTokenData.Split('$');
            if (parts.Length != 2) return false;

            string storedToken = parts[0];
            DateTime expiry = DateTime.Parse(parts[1], null, System.Globalization.DateTimeStyles.RoundtripKind);

            bool isValid = storedToken == token && expiry > DateTime.UtcNow;

            if (isValid)
            {
                await manager.RemoveAuthenticationTokenAsync(user, "Default", purpose);
            }

            return isValid;
        }
    }
}
