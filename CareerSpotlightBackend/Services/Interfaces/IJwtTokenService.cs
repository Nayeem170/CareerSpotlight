namespace CareerSpotlightBackend.Services.Interfaces
{
    public interface IJwtTokenService
    {
        (string accessToken, string refreshToken) GenerateTokens(string email);
        string GenerateAccessToken(string email);
        string? GetEmail(string refreshToken);
    }
}
