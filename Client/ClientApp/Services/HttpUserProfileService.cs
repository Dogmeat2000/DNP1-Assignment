using System.Text.Json;
using ApiContracts;

namespace ClientApp.Services;

public class HttpUserProfileService : IUserProfileService {
    private readonly HttpClient client;
    
    public HttpUserProfileService(HttpClient client) {
        this.client = client;
    }
    
    public async Task<UserProfileDTO> AddUserProfileAsync(UserProfileDTO request) {
        throw new NotImplementedException();
        // TODO: Missing Implementation
    }

    public async Task UpdateUserProfileAsync(int id, UserProfileDTO request) {
        throw new NotImplementedException();
        // TODO: Missing Implementation
    }

    public async Task DeleteUserProfileAsync(int userId, UserProfileDTO request) {
        throw new NotImplementedException();
        // TODO: Missing Implementation
    }

    public async Task<UserProfileDTO> GetSingleUserProfileAsync(int userProfileId, int userId) {
        throw new NotImplementedException();
        // TODO: Missing Implementation
    }

    public async Task<IEnumerable<UserProfileDTO>> GetManyUserProfilesAsync(int? userId) {
        HttpResponseMessage httpResponse;
        
        if (userId != null)
            httpResponse = await client.GetAsync($"UserProfiles?uId={userId}");
        else
            httpResponse = await client.GetAsync("UserProfiles");
        
        string response = await httpResponse.Content.ReadAsStringAsync();
        
        if (!httpResponse.IsSuccessStatusCode) {
            throw new KeyNotFoundException(response);
        }
        
        return JsonSerializer.Deserialize<List<UserProfileDTO>>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? Enumerable.Empty<UserProfileDTO>();
    }
}