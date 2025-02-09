using CareerSpotlightBase.Models;
using System.Net.Http.Json;

namespace CareerSpotlightApp.Services
{
    public class ProfileService
    {
        private readonly HttpClient _httpClient;

        public ProfileService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Profile> GetProfileAsync(int profileId)
        {
            return await _httpClient.GetFromJsonAsync<Profile>($"api/Profile/{profileId}");
        }
    }
}
