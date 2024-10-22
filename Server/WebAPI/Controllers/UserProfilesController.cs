using ApiContracts;
using DTOconverters;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

// TODO: Investigate handling pagination and filtering!
// TODO: Investigate how to embed HATEOAS links in responses!

[ApiController]
[Route("/Users/{uId:int}")]
public class UserProfilesController : ControllerBase {
    private readonly IUserProfileRepository _userProfileRepository;

    public UserProfilesController(IUserProfileRepository userProfileRepository) {
        _userProfileRepository = userProfileRepository;
    }
    
    // EndPoints are defined below:
    
    // Create a new UserProfile (Create)
    [HttpPost(("/"), Name = "PostUserProfile")]
    public async Task<ActionResult<UserProfileDTO>> CreateUserProfile([FromBody] UserProfileDTO newUserProfile) {
        try {
            // TODO: Validate parameters/arguments!
            
            // Convert DTO to proper entity:
            UserProfile userProfile = UserProfileConverter.DTOToUserProfile(newUserProfile);
            
            // Attempt to add UserProfile to repository:
            userProfile = await _userProfileRepository.AddAsync(userProfile);
            
            // Return result, by looking up the created UserProfile:
            return CreatedAtAction(nameof(GetUserProfile), new {userProfile.Profile_id, userProfile.User_id}); // Hands over some exception throwing/handling to AspNetCore.
            
        } catch (Exception) { 
            return ValidationProblem(); // If some other problem occured:
        }
    }
    
    
    // Read an existing UserProfile (Read)
    [HttpGet(("/{uId:int}"), Name = "Get")]
    public async Task<ActionResult<PostDTO>> GetUserProfile(int pId, int uId) {
        try {
            // TODO: Validate parameters/arguments!
            
            // Attempt to retrieve UserProfile from repository:
            UserProfile? userProfile = await _userProfileRepository.GetSingleAsync(pId, uId);
            
            // Convert to DTO as response:
            var result = UserProfileConverter.UserProfileToDTO(userProfile);
            
            // return result:
            return Ok(result);
        } catch (KeyNotFoundException) { 
            return NotFound(); // If UserProfile was not found:
        } catch (Exception) { 
            return ValidationProblem(); // If some other problem occured:
        }
    }
    
    
    // Read Multiple UserProfiles (Read):
    [HttpGet(("/[controller]"), Name = "Get")]
    public ActionResult<List<UserProfileDTO>> GetUserProfiles(int uId) {
        try {
            // TODO: Validate parameters/arguments!
            
            // Query all matching UserProfile:
            IQueryable<UserProfile> userProfiles = _userProfileRepository.GetMany().Where(uP => uP.User_id == uId);
            
            // If none were found, throw error:
            if(!userProfiles.Any())
                throw new KeyNotFoundException();
            
            // Convert found objects to DTO:
            List<UserProfileDTO> results = new List<UserProfileDTO>();
            foreach (UserProfile userProfile in userProfiles)
                results.Add(UserProfileConverter.UserProfileToDTO(userProfile));
            
            // return result:
            return Ok(results);
            
        } catch (KeyNotFoundException) { 
            return NotFound(); // If UserProfile was not found:
        } catch (Exception) { 
            return ValidationProblem(); // If some other problem occured:
        }
    }
    
    
    // Replace an existing UserProfile (Update)
    [HttpPut(("/{uPId:int}"), Name = "Put")]
    public async Task<IActionResult> Put(int uPId, int uId, [FromBody] UserProfileDTO userProfile) {
        try {
            // TODO: Validate parameters/arguments!
            
            // Convert received DTO to repository entity:
            UserProfile userProfileFromClient = UserProfileConverter.DTOToUserProfile(userProfile);
        
            // Check if a UserProfile of this type already exists:
            UserProfile userProfileFromRepository = await _userProfileRepository.GetSingleAsync(uPId, uId);
            
            // Update UserProfile properties:
            userProfileFromRepository.Username = userProfileFromClient.Username;
            userProfileFromRepository.Password = userProfileFromClient.Password;
            
            // Update the entity in repository:
            await _userProfileRepository.UpdateAsync(userProfileFromRepository);

            // Return:
            return NoContent();

        } catch (KeyNotFoundException) { 
            return NotFound(); // If UserProfile was not found:
        } catch (Exception) { 
            return ValidationProblem(); // If some other problem occured:
        }
    }
    
    
    // Remove an existing UserProfile (Delete)
    [HttpDelete(("/{uPId:int}"), Name = "Delete")]
    public async  Task<IActionResult> DeletePost(int uPId, int uId) {
        try {
            // TODO: Validate parameters/arguments!
            
            await _userProfileRepository.DeleteAsync(uPId, uId);

            // Return:
            return NoContent();

        } catch (KeyNotFoundException) {
            return NotFound(); // If UserProfile was not found:
        } catch (Exception) {
            return ValidationProblem(); // If some other problem occured:
        }
    }
}