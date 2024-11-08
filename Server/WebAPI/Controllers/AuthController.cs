using ApiContracts;
using DTOconverters;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase {
    private readonly IUserProfileRepository _userProfileRepository;
    private readonly IUserRepository _userRepository;
    
    public AuthController(IUserProfileRepository userProfileRepository,
        IUserRepository userRepository) {
        _userProfileRepository = userProfileRepository;
        _userRepository = userRepository;
    }
    
    
    // Login Endpoint
    [HttpPost(("/login"), Name = "Login")]
    public async Task<ActionResult<UserDTO>> CreateComment([FromBody] LoginDTO loginRequest) {
        try {
            // Validate loginRequest:
            if (string.IsNullOrEmpty(loginRequest.Username) || string.IsNullOrWhiteSpace(loginRequest.Username))
                return ValidationProblem(); // Username is invalid.

            // Check if UserProfile with this name exists:
            UserProfile userProfile = await _userProfileRepository.GetSingleAsync(loginRequest.Username);

            // Check if the Password provided, matches the found user:
            if(userProfile.Password != loginRequest.Password)
                return Unauthorized(ValidationProblem());
            
            // Lookup the proper User entity:
            var userFound = await _userRepository.GetSingleAsync(userProfile.User_id);
            
            // Convert DTO to proper entity:
            var result = UserConverter.UserToDTO(userFound);

            // Return result, by looking up the created Comment:
            return Ok(result); // Hands over some exception throwing/handling to AspNetCore.
        }
        catch (KeyNotFoundException) {
            return NotFound(); // Unable to find this user with the specified username.
        } catch (Exception) { 
            return ValidationProblem(); // If some other problem occured:
        }
    }
    
}