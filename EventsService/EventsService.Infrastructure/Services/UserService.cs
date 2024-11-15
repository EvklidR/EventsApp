using EventsService.Application.Interfaces;
using System.Text.Json;

namespace EventsService.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> CheckUserAsync(int userId)
        {
            var requestUri = $"https://localhost:7068/auth/Auth/check_user_by_id/{userId}";

            var response = await _httpClient.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<bool>(result);
            }
            else
            {
                throw new HttpRequestException(
                    $"Error checking user: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
            }
        }
    }
}
