using System.Text.Json;
using ApiContracts;

namespace ClientApp.Services;

public class HttpUserService : IUserService {
    private readonly HttpClient client;
    
    public HttpUserService(HttpClient client) {
        this.client = client;
    }
    
    public async Task<UserDTO> AddUserAsync(UserDTO request) {
        HttpResponseMessage httpResponse = await client.PostAsJsonAsync("Users", request);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode) {
            throw new Exception(response);
        }
        return JsonSerializer.Deserialize<UserDTO>(response, new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task UpdateUserAsync(int id, UserDTO request) {
        throw new NotImplementedException();
        // TODO: Missing implementation
    }

    public async Task DeleteUserAsync(int userId, UserDTO request) {
        throw new NotImplementedException();
        // TODO: Missing implementation
    }

    public async Task<UserDTO> GetSingleUserAsync(int userId) {
        HttpResponseMessage httpResponse = await client.GetAsync($"Users/{userId}");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode) {
            throw new KeyNotFoundException(response);
        }
        
        return JsonSerializer.Deserialize<UserDTO>(response, new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task<IEnumerable<UserDTO>> GetManyUsersAsync() {
        HttpResponseMessage httpResponse = await client.GetAsync("users");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode) {
            throw new Exception(response);
        }
        
        return JsonSerializer.Deserialize<List<UserDTO>>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? Enumerable.Empty<UserDTO>();
    }
}