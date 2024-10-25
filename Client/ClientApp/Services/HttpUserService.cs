using System.Text.Json;
using ApiContracts;

namespace ClientApp.Services;

public class HttpUserService : IUserService {
    private readonly HttpClient client;
    
    public HttpUserService(HttpClient client) {
        this.client = client;
    }
    
    public async Task<UserDTO> AddUserAsync(UserDTO request) {
        HttpResponseMessage httpResponse = await client.PostAsJsonAsync("users", request);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        return JsonSerializer.Deserialize<UserDTO>(response, new JsonSerializerOptions
        {
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

    public async Task<UserDTO> GetSingleUserAsync(int userId, UserDTO request) {
        throw new NotImplementedException();
        // TODO: Missing implementation
    }

    public IQueryable<UserDTO> GetManyUsers(UserDTO request) {
        throw new NotImplementedException();
        // TODO: Missing implementation
    }
}