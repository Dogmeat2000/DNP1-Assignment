using ApiContracts;
using DTOconverters;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

// TODO: Investigate handling pagination and filtering!
// TODO: Investigate how to embed HATEOAS links in responses!

[ApiController]
[Route("[Controller]")]
public class UsersController : ControllerBase {
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository) {
        _userRepository = userRepository;
    }
    
    // EndPoints are defined below:
    
    // Create a new User (Create)
    [HttpPost(Name = "PostUser")]
    public async Task<ActionResult<UserDTO>> CreateUser([FromBody] UserDTO newUser) {
        try {
            // TODO: Validate parameters/arguments!
            
            // Convert DTO to proper entity:
            User user = UserConverter.DTOToUser(newUser);
            
            // Attempt to add User to repository:
            user = await _userRepository.AddAsync(user);
            
            // Return result, by looking up the created User:
            return CreatedAtAction(nameof(GetUser), new {uId = user.User_id}, user); // Hands over some exception throwing/handling to AspNetCore.
            
        } catch (Exception) { 
            return ValidationProblem(); // If some other problem occured
        }
    }
    
    
    // Read an existing User (Read)
    [HttpGet(("{uId:int}"), Name = "GetUser")]
    public async Task<ActionResult<UserDTO>> GetUser(int uId) {
        try {
            // TODO: Validate parameters/arguments!
            
            // Attempt to retrieve User from repository:
            User? user = await _userRepository.GetSingleAsync(uId);
            
            // Convert to DTO as response:
            var result = UserConverter.UserToDTO(user);
            
            // return result:
            return Ok(result);
        } catch (KeyNotFoundException) { 
            return NotFound(); // If User was not found
        } catch (Exception) { 
            return ValidationProblem(); // If some other problem occured
        }
    }
    
    
    // Read Multiple User (Read):
    [HttpGet(Name = "GetUsers")]
    public ActionResult<List<UserDTO>> GetUsers() {
        try {
            // TODO: Validate parameters/arguments!
            
            // Query all matching User:
            IQueryable<User> users = _userRepository.GetMany();
            
            // If none were found, throw error:
            if(!users.Any())
                throw new KeyNotFoundException();
            
            // Convert found objects to DTO:
            List<UserDTO> results = new List<UserDTO>();
            foreach (User user in users)
                results.Add(UserConverter.UserToDTO(user));
            
            // return result:
            return Ok(results);
            
        } catch (KeyNotFoundException) { 
            return NotFound(); // If User was not found
        } catch (Exception) { 
            return ValidationProblem(); // If some other problem occured
        }
    }
    
    
    // Replace an existing User (Update)
    [HttpPut(("{uId:int}"), Name = "PutUser")]
    public async Task<IActionResult> Put(int uId, [FromBody] UserDTO user) {
        try {
            // TODO: Validate parameters/arguments!
            
            // Convert received DTO to repository entity:
            User userFromClient = UserConverter.DTOToUser(user);
        
            // Check if a User of this type already exists:
            User userFromRepository = await _userRepository.GetSingleAsync(uId);
            
            // Update User properties:
            // TODO: Currently nothing to update, since uId is controller by the database. There might be something at some point in the future!
            
            // Update the entity in repository:
            await _userRepository.UpdateAsync(userFromRepository);

            // Return:
            return NoContent();

        } catch (KeyNotFoundException) { 
            return NotFound(); // If User was not found
        } catch (Exception) { 
            return ValidationProblem(); // If some other problem occured
        }
    }
    
    
    // Remove an existing User (Delete)
    [HttpDelete(("{uId:int}"), Name = "DeleteUser")]
    public async  Task<IActionResult> DeletePost(int uId) {
        try {
            // TODO: Validate parameters/arguments!
            
            await _userRepository.DeleteAsync(uId);

            // Return:
            return NoContent();

        } catch (KeyNotFoundException) {
            return NotFound(); // If User was not found:
        } catch (Exception) {
            return ValidationProblem(); // If some other problem occured
        }
    }
}