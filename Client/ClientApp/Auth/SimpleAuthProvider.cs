using System.Security.Claims;
using System.Text.Json;
using ApiContracts;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace ClientApp.Auth;

public class SimpleAuthProvider : AuthenticationStateProvider {
    private readonly HttpClient httpClient;
    private readonly IJSRuntime jsRuntime;

    
    public SimpleAuthProvider(HttpClient httpClient, IJSRuntime jsRuntime) {
        this.httpClient = httpClient;
        this.jsRuntime = jsRuntime;
    }

    
    public override async Task<AuthenticationState> GetAuthenticationStateAsync() {
        string userAsJson = "";
        
        try {
            userAsJson = await jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "currentUser");
        } catch (InvalidOperationException e) {
            return new AuthenticationState(new());
        }

        if (string.IsNullOrEmpty(userAsJson))
            return new AuthenticationState(new());

        UserDTO userDto = JsonSerializer.Deserialize<UserDTO>(userAsJson)!;
        
        List<Claim> claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, userDto.Username),
            new Claim(ClaimTypes.NameIdentifier, userDto.User_id.ToString()),
        };
        
        ClaimsIdentity identity = new ClaimsIdentity(claims, "apiauth");
        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);
        return new AuthenticationState(claimsPrincipal);
    }

    
    // Call the web api's Login endpoint with a UserProfile attached, which contains the Username and Password:
    public async Task Login(string userName, string password) {
        
        HttpResponseMessage response = await httpClient.PostAsJsonAsync(
            "auth/login",
            new LoginDTO() {
                Password = password,
                Username = userName
            });

        // Read the content of the response. This a string, either: a. An error message, e.g. if the username or password was incorrect b. A list of claims, representing the user object, as JSON.
        string content = await response.Content.ReadAsStringAsync();

        // If the request was unsuccessful, we throw an exception, which can be caught in a page, and the error message can be displayed to the user.
        if (!response.IsSuccessStatusCode)
            throw new Exception(response.StatusCode.ToString());
        
        // Otherwise, we deserialize the JSON into a UserDto. Notice the JSON options parameter, this is because the JSON uses camelCase,
        // but my UserDto properties are PascalCase. So, we must tell the serializer to ignore casing, when looking for the correct properties to put values into.
        UserDTO userDto = JsonSerializer.Deserialize<UserDTO>(content, new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true
        })!;

        string serialisedData = JsonSerializer.Serialize(userDto);
        await jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", serialisedData);
        
        // The UserDto is converted to a list of claims.
        List<Claim> claims = new List<Claim> {
            new Claim(ClaimTypes.Name, userDto.Username ?? "ERROR: Failed to load Username"),
            new Claim("Id", userDto.User_id.ToString()),
            // Add more claims here with your own claim type as a string, e.g.:
            // new Claim("DateOfBirth", userDto.DateOfBirth.ToString("yyyy-MM-dd"))
            // new Claim("IsAdmin", userDto.IsAdmin.ToString())
            // new Claim("IsModerator", userDto.IsModerator.ToString())
            // new Claim("Email", userDto.Email)
        };

        // The list of claims is packaged into a ClaimsIdentity.
        ClaimsIdentity identity = new ClaimsIdentity(claims, "apiauth");

        // The ClaimsIdentity is packaged into a ClaimsPrincipal, which is saved in the field variable.
        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

        // And finally, we notify the app about a change in authentication state. Blazor will then update and change accordingly. Maybe some pages/components/whatever is now visible, or hidden, based on the state.
        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(claimsPrincipal))
        );
    }
    
    
    public async Task Logout() {
        await jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", "");
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new())));
    }
}